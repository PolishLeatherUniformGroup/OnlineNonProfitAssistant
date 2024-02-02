using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public sealed class ApplicationApprovedDomainEvent : DomainEventBase
{
    public DateTime DecisionDate { get; private set; }
    public NonEmptyString Email { get;  private set; }

    public ApplicationApprovedDomainEvent(DateTime decisionDate, NonEmptyString email)
    {
        this.DecisionDate = decisionDate;
        this.Email = email;
    }

    private ApplicationApprovedDomainEvent(Guid aggregateId, Guid? tenantId, DateTime decisionDate, NonEmptyString email) : base(aggregateId, tenantId)
    {
        this.DecisionDate = decisionDate;
        this.Email = email;
    }

    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationApprovedDomainEvent(aggregateId, tenantId, this.DecisionDate, this.Email);
    }
}