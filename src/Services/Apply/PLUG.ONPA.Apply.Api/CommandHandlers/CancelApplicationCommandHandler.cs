using LanguageExt.Common;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Domain.Exceptions;

namespace PLUG.ONPA.Apply.Api.CommandHandlers;

public sealed class CancelApplicationCommandHandler : CommandHandlerBase<CancelApplicationCommand>
{
    private readonly IAggregateRepository<Application> aggregateRepository;

    public CancelApplicationCommandHandler(IAggregateRepository<Application> aggregateRepository)
    {
        this.aggregateRepository = aggregateRepository;
    }

    public override async Task<Result<bool>> Handle(CancelApplicationCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            var aggregate =
                await this.aggregateRepository.GetByIdAsync(request.ApplicationId, request.TenantId, cancellationToken);
            if (aggregate is null)
            {
                return new Result<bool>(
                    new AggregateNotFoundException($"ApplicationForm {request.ApplicationId} not found"));
            }

            aggregate.CancelApplication(request.CancellationDate);

            await this.aggregateRepository.SaveAsync(aggregate, cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            return new Result<bool>(new Exception(ex.Message));
        }
    }
}