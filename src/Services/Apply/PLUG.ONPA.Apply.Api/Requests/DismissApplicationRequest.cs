namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record DismissApplicationRequest(Guid ApplicationId, Guid? TenantId, List<string> ValidRecommendations);