using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public class ApplicationRecommendedChangeEvent : ChangeEventBase
{
    public ApplicationStatus Status { get; private set; }
    
    public ApplicationRecommendedChangeEvent(ApplicationStatus status)
    {
        Status = status;
    }
    
    private ApplicationRecommendedChangeEvent(Guid aggregateId, long version, Guid? tenantId, ApplicationStatus status) : base(aggregateId, version, tenantId)
    {
        Status = status;
    }
    
    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationRecommendedChangeEvent(aggregateId, version, tenantId, Status);
    }
}