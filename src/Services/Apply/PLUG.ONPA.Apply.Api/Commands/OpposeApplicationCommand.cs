using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record OpposeApplicationCommand(Guid? TenantId, Guid ApplicationId, Guid RecommendationId) : CommandBase(TenantId);