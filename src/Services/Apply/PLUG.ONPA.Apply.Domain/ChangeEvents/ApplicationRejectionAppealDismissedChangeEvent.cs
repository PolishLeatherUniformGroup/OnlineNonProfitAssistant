using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public sealed class ApplicationRejectionAppealDismissedChangeEvent : ChangeEventBase
{
    public DateTime AppealDate { get; private set; }
    public ApplicationStatus Status { get; private set; }
    
    public ApplicationRejectionAppealDismissedChangeEvent(DateTime appealDate, ApplicationStatus status)
    {
        this.AppealDate = appealDate;
        this.Status = status;
    }

    private ApplicationRejectionAppealDismissedChangeEvent(Guid aggregateId, long version, Guid? tenantId, DateTime appealDate, ApplicationStatus status) : base(aggregateId, version, tenantId)
    {
        this.AppealDate = appealDate;
        this.Status = status;
    }

    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationRejectionAppealDismissedChangeEvent(aggregateId, version, tenantId, this.AppealDate, this.Status);
    }
}