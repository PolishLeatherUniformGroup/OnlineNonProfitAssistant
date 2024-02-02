namespace PLUG.ONPA.Apply.Api.Responses.Dtos;

public sealed class AddressDto
{
    public string Country { get; private set; }
    public string? State { get; private set; }
    public string City { get; private set; }
    public string PostalCode { get; private set; }
    public string Street { get; private set; }
    public string? AddressLine2 { get; private set; }
}

public sealed class MoneyDto
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }
}