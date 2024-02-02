using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Apply.Api.Repositories;

public sealed class ApplicationAggregateRepository : IAggregateRepository<Domain.Model.ApplicationAggregate>
{
    public Task<Domain.Model.ApplicationAggregate?> GetByIdAsync(Guid aggregateId, Guid? tenantId = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Domain.Model.ApplicationAggregate?> GetByIdForVersionAsync(Guid aggregateId, long version, Guid? tenantId = null,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task SaveAsync(Domain.Model.ApplicationAggregate aggregateRoot, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Domain.Model.ApplicationAggregate aggregateRoot, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}