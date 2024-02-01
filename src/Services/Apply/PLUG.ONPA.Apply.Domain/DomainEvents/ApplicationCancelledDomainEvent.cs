using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public class ApplicationCancelledDomainEvent :DomainEventBase
{
    public NonEmptyString Email { get; private set; }
    public DateTime CancellationDate { get; private set; }
    
    public ApplicationCancelledDomainEvent(NonEmptyString email, DateTime cancellationDate)
    {
        this.Email = email;
        this.CancellationDate = cancellationDate;
    }
    
    private ApplicationCancelledDomainEvent(Guid aggregateId, Guid? tenantId, NonEmptyString email, DateTime cancellationDate) : base(aggregateId, tenantId)
    {
        this.Email = email;
        this.CancellationDate = cancellationDate;
    }
    
    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationCancelledDomainEvent(aggregateId, tenantId, this.Email, this.CancellationDate);
    }
}