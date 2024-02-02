using PLUG.ONPA.Apply.Api.Dtos;

namespace PLUG.ONPA.Apply.Api.Requests;

public sealed record SendApplicationRequest(
    Guid? TenantId,
    string FirstName,
    string LastName,
    string Email,
    string Phone,
    AddressDto Address,
    DateTime BirthDate,
    DateTime ApplicationDate,
    List<string> Recommendations);