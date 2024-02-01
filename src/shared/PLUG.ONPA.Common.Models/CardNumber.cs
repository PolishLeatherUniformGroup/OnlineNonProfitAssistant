using PLUG.ONPA.Common.Domain;

namespace PLUG.ONPA.Common.Models;

public class CardNumber : ValueObject
{
    public NonEmptyString Prefix { get; set; }
    public int Number { get; set; }
    
    public CardNumber(NonEmptyString prefix, int number)
    {
        this.Prefix = prefix;
        this.Number = number;
    }
    
    public static implicit operator string(CardNumber cardNumber)
    {
        return $"{cardNumber.Prefix}{cardNumber.Number:D4}";
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Prefix;
        yield return this.Number;
    }
}