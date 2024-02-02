using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public sealed class ApplicationEndorsedDomainEvent : DomainEventBase
{
    public NonEmptyString Email { get; private set; }

    public ApplicationEndorsedDomainEvent(NonEmptyString email)
    {
        this.Email = email;
    }

    private ApplicationEndorsedDomainEvent(Guid aggregateId, Guid? tenantId, NonEmptyString email) : base(aggregateId,
        tenantId)
    {
        this.Email = email;
    }

    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationEndorsedDomainEvent(aggregateId, tenantId, this.Email);
    }
}