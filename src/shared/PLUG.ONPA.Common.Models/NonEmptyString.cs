using PLUG.ONPA.Common.Domain;

namespace PLUG.ONPA.Common.Models;

public class NonEmptyString :ValueObject
{
    public string Value { get; set; }
    
    public NonEmptyString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(value));
        }
        
        this.Value = value;
    }

    public override string ToString()
    {
        return this.Value;
    }

    public static implicit operator string(NonEmptyString nonEmptyString)
    {
        return nonEmptyString.Value;
    }
    
    public static implicit operator NonEmptyString(string value)
    {
        return new NonEmptyString(value);
    }
    
    public static bool operator ==(NonEmptyString left, NonEmptyString right)
    {
        return left.Value== right.Value;
    }
    
    public static bool operator !=(NonEmptyString left, NonEmptyString right)
    {
        return !(left==right);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return this.Value;
    }
}