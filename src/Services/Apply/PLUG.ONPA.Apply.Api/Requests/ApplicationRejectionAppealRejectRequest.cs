namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApplicationRejectionAppealRejectRequest(Guid ApplicationId, Guid? TenantId, DateTime RejectionDate, string Reason);