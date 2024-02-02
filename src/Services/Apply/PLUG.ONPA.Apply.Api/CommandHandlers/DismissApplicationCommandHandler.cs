using LanguageExt.Common;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Domain.Exceptions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Api.CommandHandlers;

public sealed class DismissApplicationCommandHandler : CommandHandlerBase<DismissApplicationCommand>
{
    private readonly IAggregateRepository<Application> aggregateRepository;

    public DismissApplicationCommandHandler(IAggregateRepository<Application> aggregateRepository)
    {
        this.aggregateRepository = aggregateRepository;
    }

    public override async Task<Result<Guid>> Handle(DismissApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var aggregate =
                await this.aggregateRepository.GetByIdAsync(request.ApplicationId, request.TenantId, cancellationToken);
            if (aggregate == null)
            {
                return new Result<Guid>(new AggregateNotFoundException($"ApplicationForm { request.ApplicationId} not found"));
            }
            aggregate.DismissApplication(request.ValidRecommendations.Select(x=>(CardNumber)x).ToList());
            await this.aggregateRepository.SaveAsync(aggregate, cancellationToken);
            return aggregate.AggregateId;
        }catch (Exception e)
        {
            return new Result<Guid>(e);
        }
    }
}