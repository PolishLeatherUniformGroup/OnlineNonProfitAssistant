using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record ApproveApplicationRejectionAppealCommand(Guid? TenantId, Guid ApplicationId, DateTime ApproveDate, string Reason) : CommandBase(TenantId);