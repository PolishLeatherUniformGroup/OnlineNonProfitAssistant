namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApproveApplicationRequest(Guid ApplicationId, Guid? TenantId, DateTime ApprovalDate);