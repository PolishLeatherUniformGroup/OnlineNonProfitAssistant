using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public sealed record CreateApplicationCommand(
    Guid? TenantId,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    string AddressCountry,
    string AddressCity,
    string AddressPostalCode,
    string AddressStreet,
    string? AddressLine2,
    string? AddressState,
    DateOnly BirthDate,
    DateTime ApplicationDate,
    List<string> Recommendations) : CommandBase(TenantId);
