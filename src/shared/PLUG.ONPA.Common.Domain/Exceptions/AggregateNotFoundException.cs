namespace PLUG.ONPA.Common.Domain.Exceptions;

public class AggregateNotFoundException : DomainException
{
    public AggregateNotFoundException(string message) : base(message)
    {
    }
}