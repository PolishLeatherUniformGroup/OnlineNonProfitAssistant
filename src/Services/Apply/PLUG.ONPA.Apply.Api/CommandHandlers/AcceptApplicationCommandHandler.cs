using LanguageExt.Common;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Domain.Exceptions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Api.CommandHandlers;

public sealed class AcceptApplicationCommandHandler : CommandHandlerBase<AcceptApplicationCommand>
{
    private readonly IAggregateRepository<Domain.Model.Domain> aggregateRepository;

    public AcceptApplicationCommandHandler(IAggregateRepository<Domain.Model.Domain> aggregateRepository)
    {
        this.aggregateRepository = aggregateRepository;
    }

    public override async Task<Result<Guid>> Handle(AcceptApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var aggregate =
                await this.aggregateRepository.GetByIdAsync(request.ApplicationId, request.TenantId, cancellationToken);
            if (aggregate is null)
            {
                return new Result<Guid>(new AggregateNotFoundException($"ApplicationForm {request.ApplicationId} not found"));
            }

            aggregate.AcceptApplication(new Money(request.RequiredFeeAmount, request.RequiredFeeCurrency));

            await this.aggregateRepository.SaveAsync(aggregate, cancellationToken);
            return aggregate.AggregateId;
        }
        catch (Exception e)
        {
            return new Result<Guid>(e);
        }
    }
}