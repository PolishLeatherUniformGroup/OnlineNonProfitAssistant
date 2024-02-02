using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Api.Repositories;

public sealed class ApplicationAggregateRepository : IAggregateRepository<Application>
{
    public Task<Application?> GetByIdAsync(Guid aggregateId, Guid? tenantId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Application?> GetByIdForVersionAsync(Guid aggregateId, long version, Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(Application aggregateRoot, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Application aggregateRoot, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}