using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public sealed class ApplicationRejectionAppealReceivedDomainEvent : DomainEventBase
{
    public NonEmptyString Email { get; private set; }
    public DateTime AppealDate { get; private set; }
    public NonEmptyString AppealReason { get; private set; }
    
    public ApplicationRejectionAppealReceivedDomainEvent(NonEmptyString email, DateTime appealDate, NonEmptyString appealReason)
    {
        this.Email = email;
        this.AppealDate = appealDate;
        this.AppealReason = appealReason;
    }
    
    private ApplicationRejectionAppealReceivedDomainEvent(Guid aggregateId, Guid? tenantId, NonEmptyString email, DateTime appealDate, NonEmptyString appealReason) : base(aggregateId, tenantId)
    {
        this.Email = email;
        this.AppealDate = appealDate;
        this.AppealReason = appealReason;
    }
    
    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationRejectionAppealReceivedDomainEvent(aggregateId, tenantId, this.Email, this.AppealDate, this.AppealReason);
    }
}