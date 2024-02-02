using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record AppealApplicationRejectionCommand(Guid? TenantId, Guid ApplicationId, string Reason, DateTime AppealDate) : CommandBase(TenantId);