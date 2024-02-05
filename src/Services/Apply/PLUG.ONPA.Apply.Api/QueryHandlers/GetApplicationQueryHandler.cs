using LanguageExt.Common;
using PLUG.ONPA.Apply.Api.Queries;
using PLUG.ONPA.Apply.Read.Models;
using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Common.Application.Abstractions;

namespace PLUG.ONPA.Apply.Api.QueryHandlers;

public sealed class GetApplicationQueryHandler : QueryHandlerBase<GetApplicationQuery,Application>
{
    private readonly IReadOnlyRepository<Application> repository;

    public GetApplicationQueryHandler(IReadOnlyRepository<Application> repository)
    {
        this.repository = repository;
    }

    public override async Task<Result<Application>> Handle(GetApplicationQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var application = await this.repository.GetByIdAsync(request.ApplicationId, cancellationToken);
            return application;
        }
        catch(Exception ex)
        {
            return new Result<Application>(ex);
        }
    }
}