using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public sealed class ApplicationAcceptedChangeEvent : ChangeEventBase
{
    public ApplicationStatus Status { get; private set; }
    public Money RequiredFee { get; private set; }
    
    public ApplicationAcceptedChangeEvent(ApplicationStatus status, Money requiredFee)
    {
        this.Status = status;
        this.RequiredFee = requiredFee;
    }
    
    private ApplicationAcceptedChangeEvent(Guid aggregateId, long version, Guid? tenantId, ApplicationStatus status, Money requiredFee) : base(aggregateId, version, tenantId)
    {
        this.Status = status;
        this.RequiredFee = requiredFee;
    }


    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationAcceptedChangeEvent(aggregateId, version, tenantId, this.Status, this.RequiredFee);
    }
}