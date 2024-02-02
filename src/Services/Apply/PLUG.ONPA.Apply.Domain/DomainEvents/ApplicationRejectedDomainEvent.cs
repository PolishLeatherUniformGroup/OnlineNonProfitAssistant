using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public sealed class ApplicationRejectedDomainEvent : DomainEventBase
{
    public DateTime DecisionDate { get; private set; }
    public NonEmptyString Email { get;  private set; }
    public NonEmptyString RejectionReason { get; private set; }
    public DateTime AppealDeadline { get; private set; }

    public ApplicationRejectedDomainEvent(DateTime decisionDate, NonEmptyString email, NonEmptyString rejectionReason, DateTime appealDeadline)
    {
        this.DecisionDate = decisionDate;
        this.Email = email;
        this.RejectionReason = rejectionReason;
        this.AppealDeadline = appealDeadline;
    }

    private ApplicationRejectedDomainEvent(Guid aggregateId, Guid? tenantId, DateTime decisionDate, NonEmptyString email, NonEmptyString rejectionReason, DateTime appealDeadline) : base(aggregateId, tenantId)
    {
        this.DecisionDate = decisionDate;
        this.Email = email;
        this.RejectionReason = rejectionReason;
        this.AppealDeadline = appealDeadline;
    }

    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationRejectedDomainEvent(aggregateId, tenantId, this.DecisionDate, this.Email, this.RejectionReason, this.AppealDeadline);
    }
}