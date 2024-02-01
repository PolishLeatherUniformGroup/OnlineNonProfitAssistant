using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public class ApplicationCancelledChangeEvent : ChangeEventBase
{
    public DateTime CancellationDate { get; private set; }
    public ApplicationStatus Status { get; private set; }

    public ApplicationCancelledChangeEvent(DateTime cancellationDate, ApplicationStatus status)
    {
        CancellationDate = cancellationDate;
        Status = status;
    }

    private ApplicationCancelledChangeEvent(Guid aggregateId, long version, Guid? tenantId, DateTime cancellationDate, ApplicationStatus status) : base(aggregateId, version, tenantId)
    {
        CancellationDate = cancellationDate;
        Status = status;
    }


    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationCancelledChangeEvent(aggregateId, version, tenantId, CancellationDate, Status);
    }
}