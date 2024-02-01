using PLUG.ONPA.Common.Domain;

namespace PLUG.ONPA.Common.Models;

public class Money : ValueObject
{
    public const string Pln = "PLN";
    public const string Eur = "EUR";
    public const string Usd = "USD";
    public decimal Amount { get; private set; }
    public NonEmptyString Currency { get;  private set; }

    public Money(decimal amount, string currency)
    {
        this.Amount = amount;
        this.Currency = currency;
    }
    
    public void Deconstruct(out decimal amount, out string currency)
    {
        amount = this.Amount;
        currency = this.Currency;
    }

    public override string ToString()
    {
        return $"{this.Amount:F2} {this.Currency}";
    }

    public static Money FromPln(decimal amount)
    {
        return new Money(amount, Pln);
    }
    
    public static Money FromEur(decimal amount)
    {
        return new Money(amount, Eur);
    }
    
    public static Money FromUsd(decimal amount)
    {
        return new Money(amount, Usd);
    }
    
    public static Money operator +(Money money1, Money money2)
    {
        if (money1.Currency != money2.Currency)
        {
            throw new InvalidOperationException("Cannot add money of different currencies");
        }

        return new Money(money1.Amount + money2.Amount, money1.Currency);
    }
    
    public static Money operator -(Money money1, Money money2)
    {
        if (money1.Currency != money2.Currency)
        {
            throw new InvalidOperationException("Cannot subtract money of different currencies");
        }

        return new Money(money1.Amount - money2.Amount, money1.Currency);
    }
    
    public static Money operator *(Money money, decimal multiplier)
    {
        return new Money(money.Amount * multiplier, money.Currency);
    }
    
    public static Money operator /(Money money, decimal divisor)
    {
        return new Money(money.Amount / divisor, money.Currency);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Amount;
        yield return this.Currency;
    }
}