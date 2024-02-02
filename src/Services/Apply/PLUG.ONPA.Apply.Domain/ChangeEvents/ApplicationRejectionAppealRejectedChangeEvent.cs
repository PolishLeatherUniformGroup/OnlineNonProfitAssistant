using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public sealed class ApplicationRejectionAppealRejectedChangeEvent : ChangeEventBase
{
    public DateTime DecisionDate { get; private set; }
    public string DecisionReason { get; private set; }
    public ApplicationStatus Status { get; private set; }
    
    public ApplicationRejectionAppealRejectedChangeEvent(DateTime decisionDate, string decisionReason, ApplicationStatus status)
    {
        this.DecisionDate = decisionDate;
        this.DecisionReason = decisionReason;
        this.Status = status;
    }
    
    private ApplicationRejectionAppealRejectedChangeEvent(Guid aggregateId, long version, Guid? tenantId, DateTime decisionDate, string decisionReason, ApplicationStatus status) : base(aggregateId, version, tenantId)
    {
        this.DecisionDate = decisionDate;
        this.DecisionReason = decisionReason;
        this.Status = status;
    }
    
    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationRejectionAppealRejectedChangeEvent(aggregateId, version, tenantId, this.DecisionDate, this.DecisionReason, this.Status);
    }
}