using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public class ApplicationRecommendationRequestedDomainEvent :DomainEventBase
{
    public CardNumber RecommenderCard { get; private set; }
    public DateTime RequestedAt { get; private set; }

    public ApplicationRecommendationRequestedDomainEvent(CardNumber recommenderCard, DateTime requestedAt)
    {
        this.RecommenderCard = recommenderCard;
        this.RequestedAt = requestedAt;
    }

    private ApplicationRecommendationRequestedDomainEvent(Guid aggregateId, Guid? tenantId, CardNumber recommenderCard, DateTime requestedAt) : base(aggregateId, tenantId)
    {
        this.RecommenderCard = recommenderCard;
        this.RequestedAt = requestedAt;
    }

    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationRecommendationRequestedDomainEvent(aggregateId, tenantId, this.RecommenderCard, this.RequestedAt);
    }
}