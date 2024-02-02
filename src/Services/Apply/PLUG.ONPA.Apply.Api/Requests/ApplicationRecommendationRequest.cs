namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApplicationRecommendationRequest(Guid ApplicationId, Guid? TenantId, DateTime RequestDate);