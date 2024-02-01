namespace PLUG.ONPA.Common.Domain.Abstractions;

public interface IChangeEvent
{
    Guid EventId { get; }
    Guid AggregateId { get; }
    Guid? TenantId { get; }
    long Version { get; }
    DateTime Timestamp { get; }
    Type AggregateType { get; }
    IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null);

}