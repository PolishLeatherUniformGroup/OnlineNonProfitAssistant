namespace PLUG.ONPA.Common.Domain.Exceptions;


public class UnknownTenantException : DomainException
{
    public UnknownTenantException()
    {
    }

    public UnknownTenantException(string message) : base(message)
    {
    }

    public UnknownTenantException(string message, Exception inner) : base(message, inner)
    {
    }
}

public class AggregateVersionMismatchException : Exception
{
    public AggregateVersionMismatchException()
    {
    }

    public AggregateVersionMismatchException(string message) : base(message)
    {
    }

    public AggregateVersionMismatchException(string message, Exception inner) : base(message, inner)
    {
    }
}