using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public class ApplicationRejectionAppealReceivedChangeEvent : ChangeEventBase
{
    public DateTime AppealDate { get; private set; }
    public string AppealReason { get; private set; }
    public ApplicationStatus Status { get; private set; }
    
    public ApplicationRejectionAppealReceivedChangeEvent(DateTime appealDate, string appealReason, ApplicationStatus status)
    {
        this.AppealDate = appealDate;
        this.AppealReason = appealReason;
        this.Status = status;
    }
    
    private ApplicationRejectionAppealReceivedChangeEvent(Guid aggregateId, long version, Guid? tenantId, DateTime appealDate, string appealReason, ApplicationStatus status) : base(aggregateId, version, tenantId)
    {
        AppealDate = appealDate;
        AppealReason = appealReason;
        Status = status;
    }
    
    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationRejectionAppealReceivedChangeEvent(aggregateId, version, tenantId, AppealDate, AppealReason, Status);
    }
}