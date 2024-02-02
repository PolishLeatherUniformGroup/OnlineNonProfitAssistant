using LanguageExt.Common;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Api.Services;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Domain.Exceptions;

namespace PLUG.ONPA.Apply.Api.CommandHandlers;

public sealed class RejectApplicationCommandHandler : CommandHandlerBase<RejectApplicationCommand>
{
    private readonly IAggregateRepository<Application> aggregateRepository;
    private readonly ITenantSettingsService tenantSettingsService;

    public RejectApplicationCommandHandler(IAggregateRepository<Application> aggregateRepository, ITenantSettingsService tenantSettingsService)
    {
        this.aggregateRepository = aggregateRepository;
        this.tenantSettingsService = tenantSettingsService;
    }

    public override async Task<Result<Guid>> Handle(RejectApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var aggregate =
                await this.aggregateRepository.GetByIdAsync(request.ApplicationId, request.TenantId, cancellationToken);
            if (aggregate is null)
            {
                return new Result<Guid>(new AggregateNotFoundException($"ApplicationForm {request.ApplicationId} not found"));
            }
            if(request.TenantId is null)
            {
                return new Result<Guid>(new UnknownTenantException("TenantId is null"));
            }
            var appealPeriod = await this.tenantSettingsService.GetTenantAppealPeriod(request.TenantId.Value, cancellationToken);
            
            aggregate.RejectApplication(request.DecisionDate, request.Reason, request.DecisionDate.Add(appealPeriod));
            
            await this.aggregateRepository.SaveAsync(aggregate, cancellationToken);
            return aggregate.AggregateId;
            
        }catch(Exception e)
        {
            return new Result<Guid>(e);
        }
    }
}