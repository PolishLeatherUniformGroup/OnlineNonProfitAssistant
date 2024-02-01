using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;


public class ApplicationApprovedChangeEvent : ChangeEventBase
{
    public DateTime DecisionDate { get; private set; }
    public ApplicationStatus Status { get; private set; }

    public ApplicationApprovedChangeEvent(DateTime decisionDate, ApplicationStatus status)
    {
        DecisionDate = decisionDate;
        Status = status;
    }

    private ApplicationApprovedChangeEvent(Guid aggregateId, long version, Guid? tenantId, DateTime decisionDate, ApplicationStatus status) : base(aggregateId, version, tenantId)
    {
        DecisionDate = decisionDate;
        Status = status;
    }


    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationApprovedChangeEvent(aggregateId, version, tenantId, DecisionDate, Status);
    }
}