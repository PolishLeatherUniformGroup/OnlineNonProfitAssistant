namespace PLUG.ONPA.Apply.Api.Responses.Dtos;

public sealed class ApplicationDto
{
    public string Id { get; set; }
    public string? TenantId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public AddressDto Address { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime ApplicationDate { get; set; }
}