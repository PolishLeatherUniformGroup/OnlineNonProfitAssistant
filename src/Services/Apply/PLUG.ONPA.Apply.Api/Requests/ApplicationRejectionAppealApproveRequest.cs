namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApplicationRejectionAppealApproveRequest(Guid ApplicationId, Guid? TenantId, DateTime ApprovalDate, string Reason);