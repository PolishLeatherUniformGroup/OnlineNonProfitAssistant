using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public sealed class ApplicationAcceptedDomainEvent : DomainEventBase
{
    public Money RequiredFee { get; private set; }
    public NonEmptyString Email { get; private set; }

    public ApplicationAcceptedDomainEvent(Money requiredFee, NonEmptyString email)
    {
        this.RequiredFee = requiredFee;
        this.Email = email;
    }

    private ApplicationAcceptedDomainEvent(Guid aggregateId, Guid? tenantId, Money requiredFee, NonEmptyString email) : base(aggregateId, tenantId)
    {
        this.RequiredFee = requiredFee;
        this.Email = email;
    }

    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationAcceptedDomainEvent(aggregateId,
            tenantId,
            this.RequiredFee,
            this.Email);
    }
}