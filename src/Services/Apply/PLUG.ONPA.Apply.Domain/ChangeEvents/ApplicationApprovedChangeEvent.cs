using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;


public sealed class ApplicationApprovedChangeEvent : ChangeEventBase
{
    public DateTime DecisionDate { get; private set; }
    public ApplicationStatus Status { get; private set; }

    public ApplicationApprovedChangeEvent(DateTime decisionDate, ApplicationStatus status)
    {
        this.DecisionDate = decisionDate;
        this.Status = status;
    }

    private ApplicationApprovedChangeEvent(Guid aggregateId, long version, Guid? tenantId, DateTime decisionDate, ApplicationStatus status) : base(aggregateId, version, tenantId)
    {
        this.DecisionDate = decisionDate;
        this.Status = status;
    }


    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationApprovedChangeEvent(aggregateId, version, tenantId, this.DecisionDate, this.Status);
    }
}