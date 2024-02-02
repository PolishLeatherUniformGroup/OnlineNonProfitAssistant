using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public sealed class ApplicationOpposedDomainEvent : DomainEventBase
{
    public NonEmptyString Email { get; private set; }

    public ApplicationOpposedDomainEvent(NonEmptyString email)
    {
        this.Email = email;
    }

    private ApplicationOpposedDomainEvent(Guid aggregateId, Guid? tenantId, NonEmptyString email) : base(aggregateId,
        tenantId)
    {
        this.Email = email;
    }

    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationOpposedDomainEvent(aggregateId, tenantId, this.Email);
    }
}