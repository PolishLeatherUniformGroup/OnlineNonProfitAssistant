namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApplicationRejectionAppealDismissRequest(Guid ApplicationId, Guid? TenantId);