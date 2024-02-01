using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public class ApplicationRejectionAppealRejectedChangeEvent : ChangeEventBase
{
    public DateTime DecisionDate { get; private set; }
    public string DecisionReason { get; private set; }
    public ApplicationStatus Status { get; private set; }
    
    public ApplicationRejectionAppealRejectedChangeEvent(DateTime decisionDate, string decisionReason, ApplicationStatus status)
    {
        DecisionDate = decisionDate;
        DecisionReason = decisionReason;
        Status = status;
    }
    
    private ApplicationRejectionAppealRejectedChangeEvent(Guid aggregateId, long version, Guid? tenantId, DateTime decisionDate, string decisionReason, ApplicationStatus status) : base(aggregateId, version, tenantId)
    {
        DecisionDate = decisionDate;
        DecisionReason = decisionReason;
        Status = status;
    }
    
    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationRejectionAppealRejectedChangeEvent(aggregateId, version, tenantId, DecisionDate, DecisionReason, Status);
    }
}