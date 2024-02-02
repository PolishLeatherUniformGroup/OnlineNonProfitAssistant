namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record RejectApplicationRequest(Guid ApplicationId, Guid? TenantId, DateTime RejectionDate, string Reason);