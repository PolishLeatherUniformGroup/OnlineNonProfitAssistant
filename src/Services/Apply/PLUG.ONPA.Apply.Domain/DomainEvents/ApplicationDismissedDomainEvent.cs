using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public sealed class ApplicationDismissedDomainEvent : DomainEventBase
{
    public NonEmptyString Email { get;  private set; }
    
    public ApplicationDismissedDomainEvent(NonEmptyString email)
    {
        this.Email = email;
    }
    
    private ApplicationDismissedDomainEvent(Guid aggregateId, Guid? tenantId, NonEmptyString email) : base(aggregateId, tenantId)
    {
        this.Email = email;
    }
    
    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationDismissedDomainEvent(aggregateId, tenantId, this.Email);
    }
}