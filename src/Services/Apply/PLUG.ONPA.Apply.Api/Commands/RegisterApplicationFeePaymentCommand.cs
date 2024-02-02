using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record RegisterApplicationFeePaymentCommand(Guid? TenantId, Guid ApplicationId, decimal Amount, string Currency, DateTime PaymentDate) : CommandBase(TenantId);