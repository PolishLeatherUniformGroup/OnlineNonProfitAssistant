using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Common.Domain;

public abstract class ChangeEventBase : IChangeEvent, IEquatable<ChangeEventBase>
{
    public Guid EventId { get; }
    public Guid AggregateId { get; }
    public Guid? TenantId { get; }
    public long Version { get; }
    public DateTime Timestamp { get; }
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    public Type AggregateType { get; }

    public abstract IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType,
        Guid? tenantId = null);

    protected ChangeEventBase()
    {
        this.EventId = Guid.NewGuid();
        this.Timestamp = DateTime.UtcNow;
    }
    
    protected ChangeEventBase(Guid aggregateId, long version, Guid? tenantId) : this()
    {
        this.AggregateId = aggregateId;
        this.Version = version;
        this.TenantId = tenantId ?? Guid.Empty;
    }
   

    public bool Equals(ChangeEventBase? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return this.EventId.Equals(other.EventId) && this.AggregateId.Equals(other.AggregateId) && Nullable.Equals(this.TenantId, other.TenantId) && this.Timestamp.Equals(other.Timestamp)
               && this.Version == other.Version;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return this.Equals((ChangeEventBase)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(this.EventId, this.AggregateId, this.TenantId, this.Timestamp, this.Version);
    }
}