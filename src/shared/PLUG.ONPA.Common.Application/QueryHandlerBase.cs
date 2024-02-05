using LanguageExt.Common;
using MediatR;
using PLUG.ONPA.Common.Application.Abstractions;

namespace PLUG.ONPA.Common.Application;

public abstract class QueryHandlerBase<TQuery, TResult> : IRequestHandler<TQuery, Result<TResult>>
    where TQuery : IQuery<TResult>
    where TResult : notnull
{
    public abstract Task<Result<TResult>> Handle(TQuery request, CancellationToken cancellationToken);
}