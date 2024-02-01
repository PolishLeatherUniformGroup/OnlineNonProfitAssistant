using LanguageExt.Common;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Api.CommandHandlers;

public class CreateApplicationFormCommandHandler : CommandHandlerBase<CreateApplicationFormCommand>
{
    private readonly IAggregateRepository<ApplicationForm> aggregateRepository;

    public CreateApplicationFormCommandHandler(IAggregateRepository<ApplicationForm> aggregateRepository)
    {
        this.aggregateRepository = aggregateRepository;
    }

    public override async Task<Result<bool>> Handle(CreateApplicationFormCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var (birthDate, _) = request.BirthDate;
            var aggregate = new ApplicationForm(
                request.FirstName,
                request.LastName,
                new Address(request.AddressCountry, request.AddressCity, request.AddressPostalCode,
                    request.AddressStreet,
                    request.AddressState, request.AddressLine2),
                birthDate,
                request.Email,
                request.Phone,
                request.Recommendations.Select(x => new ApplicationRecommendation(Guid.NewGuid(), (CardNumber) x))
                    .ToList(),
                request.ApplicationDate,
                request.TenantId
            );
            await aggregateRepository.SaveAsync(aggregate, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            return new Result<bool>(e);
        }
    }
}