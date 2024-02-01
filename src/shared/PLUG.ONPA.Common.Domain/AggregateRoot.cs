using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Domain.Exceptions;

namespace PLUG.ONPA.Common.Domain;

public abstract class AggregateRoot : IAggregateRoot
{
    public const long NewAggregateVersion = -1;
    
    public Guid AggregateId { get; protected set; }
    public Guid? TenantId { get; protected set; }
    public long Version
    {
        get => this.version;
        protected set => this.version = value;
    }

    protected long version = NewAggregateVersion;
    protected readonly ICollection<IDomainEvent> domainEvents = new LinkedList<IDomainEvent>();
    protected readonly ICollection<IChangeEvent> changeEvents = new LinkedList<IChangeEvent>();

    protected AggregateRoot(Guid? tenantId = null)
    {
        this.AggregateId = Guid.NewGuid();
        this.TenantId = tenantId ?? Guid.Empty;
    }

    protected AggregateRoot(Guid aggregateId, IEnumerable<IChangeEvent> events, Guid? tenantId)
    {
        this.AggregateId = aggregateId;
        this.TenantId = tenantId;
        foreach (var @event in events)
        {
            this.ApplyChange(@event);
        }
    }
    public IEnumerable<IChangeEvent> GetChangeEvents()
    {
        return this.changeEvents.AsEnumerable();
    }

    public void ApplyChanges()
    {
        foreach (var change in this.changeEvents)
        {
            this.ApplyChange(change);
        }
    }

    public void ClearChangeEvents()
    {
        this.changeEvents.Clear();
    }

    public IEnumerable<IDomainEvent> GetDomainEvents()
    {
        return this.domainEvents.AsEnumerable();
    }

    public void ClearDomainEvents()
    {
        this.domainEvents.Clear();
    }
    
    protected void RaiseDomainEvent<TEvent>(TEvent @event) where TEvent : DomainEventBase
    {
        var eventWithAggregate = @event.WithAggregate(this.AggregateId, this.TenantId);
        this.domainEvents.Add(eventWithAggregate);
    }
    
    protected void EmmitChangeEvent<TChange>(TChange change) where TChange :IChangeEvent
    {
        var changeWithAggregate = change.WithAggregate(this.AggregateId, this.Version, this.GetType(), this.TenantId);
        this.changeEvents.Add(changeWithAggregate);
    }
    
    protected void ApplyChange(IChangeEvent change)
    {
        if (this.changeEvents.Any(c => Equals(c.EventId, @change.EventId)))
        {
            if (this.version != change.Version)
            {
                throw new AggregateVersionMismatchException();
            }
            ((dynamic)this).Apply((dynamic)change);
            this.version++;
        }
    }
}