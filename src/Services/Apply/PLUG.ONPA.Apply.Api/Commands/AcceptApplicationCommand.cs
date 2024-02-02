using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record AcceptApplicationCommand(
    Guid? TenantId,
    Guid ApplicationId,
    decimal RequiredFeeAmount,
    string RequiredFeeCurrency) : CommandBase(TenantId);
