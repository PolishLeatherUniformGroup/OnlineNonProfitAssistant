using PLUG.ONPA.Common.Domain;

namespace PLUG.ONPA.Common.Infrastructure.EventStore.Abstractions;

public interface IEventStore
{
    Task StoreAsync(AggregateRoot aggregateRoot, CancellationToken cancellationToken = default);
    
    Task<TAggregate?> ReadAsync<TAggregate>(Guid aggregateId, CancellationToken cancellationToken = default) where TAggregate : AggregateRoot;
    Task<TAggregate?> ReadAsync<TAggregate>(Guid aggregateId, long version, CancellationToken cancellationToken = default) where TAggregate : AggregateRoot;
}