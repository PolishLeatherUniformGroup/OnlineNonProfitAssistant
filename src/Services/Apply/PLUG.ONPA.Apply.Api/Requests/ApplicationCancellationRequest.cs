namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApplicationCancellationRequest(Guid ApplicationId, Guid? TenantId, DateTime CancellationDate);