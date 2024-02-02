using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record RejectApplicationRejectionAppealCommand(Guid? TenantId, Guid ApplicationId, DateTime RejectDate, string Reason) : CommandBase(TenantId);