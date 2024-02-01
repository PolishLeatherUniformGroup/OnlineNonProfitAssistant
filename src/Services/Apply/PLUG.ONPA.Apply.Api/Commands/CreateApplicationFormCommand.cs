using PLUG.ONPA.Common.Application;

namespace PLUG.ONPA.Apply.Api.Commands;

public record CreateApplicationFormCommand(
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
    DateTime BirthDate,
    DateTime ApplicationDate,
    List<string> Recommendations) : CommandBase(TenantId);