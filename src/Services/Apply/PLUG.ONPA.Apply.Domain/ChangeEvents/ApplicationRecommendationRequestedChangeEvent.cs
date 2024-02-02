using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public sealed class ApplicationRecommendationRequestedChangeEvent :ChangeEventBase
{
    public DateTime RequestedAt { get; private set; }
    public ApplicationStatus Status { get; private set; }

    public ApplicationRecommendationRequestedChangeEvent(DateTime requestedAt, ApplicationStatus status)
    {
        this.RequestedAt = requestedAt;
        this.Status = status;
    }

    public ApplicationRecommendationRequestedChangeEvent(Guid aggregateId, long version, Guid? tenantId, DateTime requestedAt, ApplicationStatus status) : base(aggregateId, version, tenantId)
    {
        this.RequestedAt = requestedAt;
        this.Status = status;
    }

    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationRecommendationRequestedChangeEvent(aggregateId, version, tenantId, this.RequestedAt, this.Status);
    }
}