namespace PLUG.ONPA.Apply.Api.Services;

public interface ITenantSettingsService
{
    Task<TimeSpan> GetTenantAppealPeriod(Guid tenantId, CancellationToken cancellationToken=default);
}