namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record ApplicationFeeRegistrationRequest(Guid ApplicationId, Guid? TenantId, decimal FeeAmount, string FeeCurrency, DateTime PaymentDate);