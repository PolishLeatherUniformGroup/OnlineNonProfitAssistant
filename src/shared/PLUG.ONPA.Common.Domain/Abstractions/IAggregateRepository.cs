namespace PLUG.ONPA.Common.Domain.Abstractions;

public interface IAggregateRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
{
    Task<TAggregateRoot?> GetByIdAsync(Guid aggregateId, Guid? tenantId = null, CancellationToken cancellationToken = default);
    Task<TAggregateRoot?> GetByIdForVersionAsync(Guid aggregateId, long version, Guid? tenantId = null, CancellationToken cancellationToken = default);
    Task SaveAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);
    Task UpdateAsync(TAggregateRoot aggregateRoot, CancellationToken cancellationToken = default);
}