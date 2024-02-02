namespace PLUG.ONPA.Apply.Api.Dtos;

public sealed class AddressDto
{
    public string Country { get;  set; }
    public string? State { get;  set; }
    public string City { get;  set; }
    public string PostalCode { get;  set; }
    public string Street { get;  set; }
    public string? AddressLine2 { get;  set; }
}