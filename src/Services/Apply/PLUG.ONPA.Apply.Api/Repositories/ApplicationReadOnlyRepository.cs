using System.Linq.Expressions;
using PLUG.ONPA.Apply.Read.Models;
using PLUG.ONPA.Common.Application.Abstractions;

namespace PLUG.ONPA.Apply.Api.Repositories;

public sealed class ApplicationReadOnlyRepository : IReadOnlyRepository<Application>
{
    public Task<Application> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Application>> ListAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Application>> ListAsync(Expression<Func<Application, bool>> predicate, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}