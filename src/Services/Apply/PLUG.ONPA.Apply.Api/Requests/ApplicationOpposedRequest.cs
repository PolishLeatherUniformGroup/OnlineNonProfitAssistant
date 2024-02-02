namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApplicationOpposedRequest(Guid ApplicationId, Guid? TenantId, Guid RecommendationId);