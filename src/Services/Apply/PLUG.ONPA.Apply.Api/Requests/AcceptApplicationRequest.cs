namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record AcceptApplicationRequest(Guid ApplicationId, Guid? TenantId);