using PLUG.ONPA.Common.Domain;

namespace PLUG.ONPA.Common.Models;

public class Address : ValueObject
{
    public NonEmptyString Country { get; private set; }
    public string? State { get; private set; }
    public NonEmptyString City { get; private set; }
    public NonEmptyString PostalCode { get; private set; }
    public NonEmptyString Street { get; private set; }
    
    public string? AddressLine2 { get; private set; }

    public Address(string country, string city, string postalCode, string street, string? state = null, string addressLine2 = null)
    {
        this.Country = country;
        this.State = state;
        this.AddressLine2 = addressLine2;
        this.City = city;
        this.PostalCode = postalCode;
        this.Street = street;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Country;
        yield return State;
        yield return City;
        yield return PostalCode;
        yield return Street;
        yield return AddressLine2;
    }
}