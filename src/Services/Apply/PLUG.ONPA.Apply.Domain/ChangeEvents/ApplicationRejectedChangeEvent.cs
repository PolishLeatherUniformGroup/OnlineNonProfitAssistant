using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public sealed class ApplicationRejectedChangeEvent :ChangeEventBase
{
    public ApplicationStatus Status { get; private set; }
    public DateTime DecisionDate { get; private set; }
    public string DecisionReason { get; private set; }
    public DateTime AppealDeadline { get; private set; }
    
    public ApplicationRejectedChangeEvent(ApplicationStatus status, DateTime decisionDate, string decisionReason, DateTime appealDeadline)
    {
        this.Status = status;
        this.DecisionDate = decisionDate;
        this.DecisionReason = decisionReason;
        this.AppealDeadline = appealDeadline;
    }

    private ApplicationRejectedChangeEvent(Guid aggregateId, long version, Guid? tenantId, ApplicationStatus status, DateTime decisionDate, string decisionReason, DateTime appealDeadline) : base(aggregateId, version, tenantId)
    {
        this.Status = status;
        this.DecisionDate = decisionDate;
        this.DecisionReason = decisionReason;
        this.AppealDeadline = appealDeadline;
    }

    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationRejectedChangeEvent(aggregateId, version, tenantId, this.Status, this.DecisionDate, this.DecisionReason, this.AppealDeadline);
    }
}