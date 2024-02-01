using LanguageExt.Common;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Api.CommandHandlers;

public class AcceptApplicationCommandHandler : CommandHandlerBase<AcceptApplicationCommand>
{
    private readonly IAggregateRepository<ApplicationForm> aggregateRepository;

    public AcceptApplicationCommandHandler(IAggregateRepository<ApplicationForm> aggregateRepository)
    {
        this.aggregateRepository = aggregateRepository;
    }

    public override async Task<Result<bool>> Handle(AcceptApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var aggregate =
                await aggregateRepository.GetByIdAsync(request.ApplicationId, request.TenantId, cancellationToken);
            if (aggregate is null)
            {
                return new Result<bool>(new Exception("Aggregate not found"));
            }

            aggregate.AcceptApplication(new Money(request.RequiredFeeAmount, request.RequiredFeeCurrency));

            await aggregateRepository.UpdateAsync(aggregate, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            return new Result<bool>(e);
        }
    }
}