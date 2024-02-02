using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public sealed class ApplicationOpposedChangeEvent : ChangeEventBase
{
    public ApplicationStatus Status { get; private set; }
    public Guid RecommendationId { get; private set; }
    
    public ApplicationOpposedChangeEvent(ApplicationStatus status, Guid recommendationId)
    {
        this.Status = status;
        this.RecommendationId = recommendationId;
    }
    
    private ApplicationOpposedChangeEvent(Guid aggregateId, long version, Guid? tenantId, ApplicationStatus status, Guid recommendationId) : base(aggregateId, version, tenantId)
    {
        this.Status = status;
        this.RecommendationId = recommendationId;
    }
    
    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationOpposedChangeEvent(aggregateId, version, tenantId, this.Status, this.RecommendationId);
    }
}