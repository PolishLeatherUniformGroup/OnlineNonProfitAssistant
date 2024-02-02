namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApplicationRejectionAppealRequest(Guid ApplicationId, Guid? TenantId, DateTime AppealDate, string Reason);