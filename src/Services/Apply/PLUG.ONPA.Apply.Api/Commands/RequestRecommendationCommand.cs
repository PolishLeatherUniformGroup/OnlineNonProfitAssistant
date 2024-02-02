using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record RequestRecommendationCommand(Guid? TenantId, Guid ApplicationId, DateTime RequestDate): CommandBase(TenantId);