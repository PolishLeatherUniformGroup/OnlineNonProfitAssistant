using Microsoft.EntityFrameworkCore;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Infrastructure.EventStore.Abstractions;
using PLUG.ONPA.Common.Infrastructure.Storage;

namespace PLUG.ONPA.Common.Infrastructure.EventStore;

public class EventStore : IEventStore
{
    private readonly EventStreamContext eventStreamContext;

    public EventStore(EventStreamContext eventStreamContext)
    {
        this.eventStreamContext = eventStreamContext;
    }

    public async Task StoreAsync(AggregateRoot aggregateRoot, CancellationToken cancellationToken = default)
    {
        await using var transaction = await this.eventStreamContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var events = aggregateRoot.GetChangeEvents();
            foreach (var @event in events)
            {
                var eventEntry = EventEntry.WithData(@event);
                await this.eventStreamContext.EventsStream.AddAsync(eventEntry, cancellationToken);
                await this.eventStreamContext.EventsStatus.AddAsync(new EventStatus(eventEntry),cancellationToken);
            }
            var domainEvents = aggregateRoot.GetDomainEvents();
            foreach (var domainEvent in domainEvents)
            {
                var domainEventEntry = DomainEventEntry.WithData(domainEvent);
                await this.eventStreamContext.DomainEvents.AddAsync(domainEventEntry, cancellationToken);
            }

            await this.eventStreamContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
            aggregateRoot.ClearChangeEvents();
            aggregateRoot.ClearDomainEvents();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<TAggregate?> ReadAsync<TAggregate>(Guid aggregateId, CancellationToken cancellationToken = default) where TAggregate : AggregateRoot
    {
        var events = await this.eventStreamContext.EventsStream
            .Where(x => x.StreamId == aggregateId && x.StreamType == typeof(TAggregate))
            .OrderBy(o=>o.Version)  
            .ToListAsync(cancellationToken: cancellationToken);
        return this.HydrateAggregate<TAggregate>(aggregateId, events);
    }

    public async Task<TAggregate?> ReadAsync<TAggregate>(Guid aggregateId, long version, CancellationToken cancellationToken = default) where TAggregate : AggregateRoot
    {
        var events = await this.eventStreamContext.EventsStream
            .Where(x => x.StreamId == aggregateId && x.StreamType == typeof(TAggregate) && x.Version <= version)
            .OrderBy(o=>o.Version)  
            .ToListAsync(cancellationToken: cancellationToken);
        return this.HydrateAggregate<TAggregate>(aggregateId, events);
    }

    private TAggregate? HydrateAggregate<TAggregate>(Guid aggregateId, IEnumerable<EventEntry> events) where TAggregate : AggregateRoot
    {
        if (!events.Any())
        {
            return null;
        }
        var changes = events.Select(e=>e.GetData<IChangeEvent>()).ToList();
        var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate),aggregateId, changes, events.First().TenantId);
        return aggregate;
    }
}

