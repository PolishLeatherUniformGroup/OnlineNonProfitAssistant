namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApplicationRejectionAppealAcceptRequest(Guid ApplicationId, Guid? TenantId);