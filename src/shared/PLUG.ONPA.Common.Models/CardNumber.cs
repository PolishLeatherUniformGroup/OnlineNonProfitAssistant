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
    
    public static explicit operator CardNumber(string cardNumber)
    {
        var number = int.Parse(cardNumber.Substring(cardNumber.Length - 4,cardNumber.Length));
        var prefix = cardNumber.Substring(0, cardNumber.Length - 4);
        return new CardNumber(prefix, number);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Prefix;
        yield return this.Number;
    }
}