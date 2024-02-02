using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record RejectApplicationCommand(Guid? TenantId, Guid ApplicationId, DateTime DecisionDate, string Reason) : CommandBase(TenantId);