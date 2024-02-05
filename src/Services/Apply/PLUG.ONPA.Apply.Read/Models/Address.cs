namespace PLUG.ONPA.Apply.Read.Models;

public sealed class Address
{
    public string Country { get; set; }
    public string Street { get; set; }
    public string City { get; set; }
    public string? State { get; set; }
    public string ZipCode { get; set; }
    public string AddressLine2 { get; set; }
}