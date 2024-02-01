using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public class ApplicationEndorsedChangeEvent : ChangeEventBase
{
    public Guid RecommendationId { get; private set; }

    public ApplicationEndorsedChangeEvent(Guid recommendationId)
    {
        RecommendationId = recommendationId;
    }

    private ApplicationEndorsedChangeEvent(Guid aggregateId, long version, Guid? tenantId, Guid recommendationId) : base(aggregateId, version, tenantId)
    {
        RecommendationId = recommendationId;
    }

    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationEndorsedChangeEvent(aggregateId, version, tenantId, RecommendationId);
    }
}