using MediatR;

namespace PLUG.ONPA.Common.Domain.Abstractions;

public interface IDomainEvent : INotification
{
    Guid EventId { get; }
    Guid AggregateId { get; }
    Guid? TenantId { get; }
    DateTime Timestamp { get; }
}