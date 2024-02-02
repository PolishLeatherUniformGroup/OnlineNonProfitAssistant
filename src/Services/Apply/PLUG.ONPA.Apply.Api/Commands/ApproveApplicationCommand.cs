using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record ApproveApplicationCommand(Guid?TenantId, Guid ApplicationId, DateTime DecisionDate):CommandBase(TenantId);