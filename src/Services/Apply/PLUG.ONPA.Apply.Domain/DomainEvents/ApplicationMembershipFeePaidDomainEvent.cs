using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public sealed class ApplicationMembershipFeePaidDomainEvent :DomainEventBase
{
    public NonEmptyString Email { get; private set; }
    public DateTime PaidDate { get; private set; }
    public Money PaidFee { get; private set; }
    
    public ApplicationMembershipFeePaidDomainEvent(NonEmptyString email, DateTime paidDate, Money paidFee)
    {
        this.Email = email;
        this.PaidDate = paidDate;
        this.PaidFee = paidFee;
    }
    
    private ApplicationMembershipFeePaidDomainEvent(Guid aggregateId, Guid? tenantId, NonEmptyString email, DateTime paidDate, Money paidFee) : base(aggregateId, tenantId)
    {
        this.Email = email;
        this.PaidDate = paidDate;
        this.PaidFee = paidFee;
    }
    
    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationMembershipFeePaidDomainEvent(aggregateId, tenantId, this.Email, this.PaidDate, this.PaidFee);
    }
}