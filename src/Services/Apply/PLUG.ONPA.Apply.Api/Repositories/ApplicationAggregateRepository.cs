using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Api.Repositories;

public sealed class ApplicationAggregateRepository : IAggregateRepository<ApplicationAggregate>
{
    public Task<ApplicationAggregate?> GetByIdAsync(Guid aggregateId, Guid? tenantId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ApplicationAggregate?> GetByIdForVersionAsync(Guid aggregateId, long version, Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(ApplicationAggregate aggregateRoot, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(ApplicationAggregate aggregateRoot, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}