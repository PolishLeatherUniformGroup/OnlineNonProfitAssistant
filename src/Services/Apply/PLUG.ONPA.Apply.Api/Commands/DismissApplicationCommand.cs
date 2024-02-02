using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record DismissApplicationCommand(Guid? TenantId, Guid ApplicationId, List<string> ValidRecommendations) : CommandBase(TenantId);