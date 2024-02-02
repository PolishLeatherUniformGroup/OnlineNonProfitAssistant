using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record DismissApplicationRejectionAppealCommand(Guid? TenantId, Guid ApplicationId) : CommandBase(TenantId);