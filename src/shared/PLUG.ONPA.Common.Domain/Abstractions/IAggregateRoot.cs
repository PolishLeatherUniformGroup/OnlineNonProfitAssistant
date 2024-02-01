namespace PLUG.ONPA.Common.Domain.Abstractions;

public interface IAggregateRoot
{
    Guid AggregateId { get; }
    Guid? TenantId { get; }
    long Version { get; }

    IEnumerable<IChangeEvent> GetChangeEvents();
    void ClearChangeEvents();
    
    IEnumerable<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}