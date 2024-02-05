using MediatR;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Common.Application;

public abstract class DomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent> where TDomainEvent : IDomainEvent
{
    public abstract Task Handle(TDomainEvent notification, CancellationToken cancellationToken);
}