namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApplicationEndorsedRequest(Guid ApplicationId, Guid? TenantId, Guid RecommendationId);