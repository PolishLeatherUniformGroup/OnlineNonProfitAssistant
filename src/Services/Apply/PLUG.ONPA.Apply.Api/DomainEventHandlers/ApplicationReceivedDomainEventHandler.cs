using PLUG.ONPA.Apply.Domain.DomainEvents;
using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Shared.ExternalCommunication.Abstractions;

namespace PLUG.ONPA.Apply.Api.DomainEventHandlers;

public sealed class ApplicationReceivedDomainEventHandler : DomainEventHandler<ApplicationReceivedDomainEvent>
{
    private readonly IEmailService emailService;

    public ApplicationReceivedDomainEventHandler(IEmailService emailService)
    {
        this.emailService = emailService;
    }

    public override Task Handle(ApplicationReceivedDomainEvent notification, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
    
   
}