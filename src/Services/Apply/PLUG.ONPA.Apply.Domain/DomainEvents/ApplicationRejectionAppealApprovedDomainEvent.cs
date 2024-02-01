using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public class ApplicationRejectionAppealApprovedDomainEvent : DomainEventBase
{
    public NonEmptyString Email { get; private set; }
    public DateTime DecisionDate { get; private set; }
    public NonEmptyString DecisionReason { get; private set; }
    
    public ApplicationRejectionAppealApprovedDomainEvent(NonEmptyString email, DateTime decisionDate, NonEmptyString decisionReason)
    {
        this.Email = email;
        this.DecisionDate = decisionDate;
        this.DecisionReason = decisionReason;
    }
    
    private ApplicationRejectionAppealApprovedDomainEvent(Guid aggregateId, Guid? tenantId, NonEmptyString email, DateTime decisionDate, NonEmptyString decisionReason) : base(aggregateId, tenantId)
    {
        this.Email = email;
        this.DecisionDate = decisionDate;
        this.DecisionReason = decisionReason;
    }
    
    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationRejectionAppealApprovedDomainEvent(aggregateId, tenantId, this.Email, this.DecisionDate, this.DecisionReason);
    }
}