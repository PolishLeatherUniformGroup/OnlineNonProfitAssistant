using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public sealed class ApplicationDismissedChangeEvent :ChangeEventBase
{
    public ApplicationStatus Status { get; private set; }
    public List<CardNumber> ValidRecommendations { get; private set; }
    
    public ApplicationDismissedChangeEvent(ApplicationStatus status, List<CardNumber> validRecommendations)
    {
        this.Status = status;
        this.ValidRecommendations = validRecommendations;
    }

    private ApplicationDismissedChangeEvent(Guid aggregateId, long version, Guid? tenantId, ApplicationStatus status, List<CardNumber> validRecommendations) : base(aggregateId, version, tenantId)
    {
        this.Status = status;
        this.ValidRecommendations = validRecommendations;
    }


    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationDismissedChangeEvent(aggregateId, version, tenantId, this.Status, this.ValidRecommendations);
    }
}