using LanguageExt.Common;
using PLUG.ONPA.Apply.Api.Commands;
using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Application;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Domain.Exceptions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Api.CommandHandlers;

public sealed class CreateApplicationCommandHandler : CommandHandlerBase<CreateApplicationCommand>
{
    private readonly IAggregateRepository<Application> aggregateRepository;

    public CreateApplicationCommandHandler(IAggregateRepository<Application> aggregateRepository)
    {
        this.aggregateRepository = aggregateRepository;
    }

    public override async Task<Result<bool>> Handle(CreateApplicationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var (birthDate, _) = request.BirthDate;
            var aggregate = new Application(
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
            await this.aggregateRepository.SaveAsync(aggregate, cancellationToken);
            return true;
        }
        catch (DomainException e)
        {
            return new Result<bool>(e);
        }
    }
}