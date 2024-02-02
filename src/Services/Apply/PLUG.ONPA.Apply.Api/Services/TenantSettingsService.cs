namespace PLUG.ONPA.Apply.Api.Services;

public sealed class TenantSettingsService : ITenantSettingsService
{
    public Task<TimeSpan> GetTenantAppealPeriod(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(TimeSpan.FromDays(14));
    }
}