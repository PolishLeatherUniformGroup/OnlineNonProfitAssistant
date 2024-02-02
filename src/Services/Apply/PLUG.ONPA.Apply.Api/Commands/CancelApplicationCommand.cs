using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record CancelApplicationCommand(Guid? TenantId, Guid ApplicationId, DateTime CancellationDate)
    : CommandBase(TenantId);
