using LanguageExt.Common;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Domain.Exceptions;

namespace PLUG.ONPA.Apply.Api.CommandHandlers;

/// <summary>
/// For offline accept in case automated process not work.
/// </summary>
public sealed class AcceptApplicationRejectionAppealCommandHandler : CommandHandlerBase<AcceptApplicationRejectionAppealCommand>
{
    private readonly IAggregateRepository<Application> aggregateRepository;

    public AcceptApplicationRejectionAppealCommandHandler(IAggregateRepository<Application> aggregateRepository)
    {
        this.aggregateRepository = aggregateRepository;
    }

    public override async Task<Result<Guid>> Handle(AcceptApplicationRejectionAppealCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var aggregate = await this.aggregateRepository.GetByIdAsync(request.ApplicationId, request.TenantId, cancellationToken);
            if (aggregate is null)
            {
                return new Result<Guid>(
                    new AggregateNotFoundException($"Application {request.ApplicationId} not found"));
            }
            return aggregate.AggregateId;
        }
        catch (Exception ex)
        {
            return new Result<Guid>(ex);
        }
    }
}