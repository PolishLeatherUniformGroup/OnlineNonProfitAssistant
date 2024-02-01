using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Common.Domain;

public abstract class DomainEventBase : IDomainEvent, IEquatable<DomainEventBase>
{
    public Guid EventId { get; }
    public Guid AggregateId { get; }
    public Guid? TenantId { get; }
    public DateTime Timestamp { get; }

    protected DomainEventBase()
    {
        this.EventId = Guid.NewGuid();
        this.Timestamp = DateTime.UtcNow;
    }
    
    protected DomainEventBase(Guid aggregateId, Guid? tenantId) : this()
    {
        this.AggregateId = aggregateId;
        this.TenantId = tenantId;
    }
    
    public bool Equals(DomainEventBase? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return this.EventId.Equals(other.EventId) && this.AggregateId.Equals(other.AggregateId) && Nullable.Equals(this.TenantId, other.TenantId) && this.Timestamp.Equals(other.Timestamp);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((DomainEventBase)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.EventId, this.AggregateId, this.TenantId, this.Timestamp);
    }
    
    public abstract IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId);
}