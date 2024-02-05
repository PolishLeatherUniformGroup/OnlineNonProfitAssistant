using System.Linq.Expressions;

namespace PLUG.ONPA.Common.Application.Abstractions;

public interface IReadOnlyRepository<TEntity> where TEntity : notnull
{
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> ListAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
}