using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Api.Repositories;

public sealed class ApplicationAggregateRepository : IAggregateRepository<Domain.Model.Domain>
{
    public Task<Domain.Model.Domain?> GetByIdAsync(Guid aggregateId, Guid? tenantId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Domain.Model.Domain?> GetByIdForVersionAsync(Guid aggregateId, long version, Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(Domain.Model.Domain aggregateRoot, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Domain.Model.Domain aggregateRoot, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}