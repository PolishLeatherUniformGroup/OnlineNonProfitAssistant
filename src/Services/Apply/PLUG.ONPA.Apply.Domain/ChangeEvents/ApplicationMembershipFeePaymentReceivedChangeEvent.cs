using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public sealed class ApplicationMembershipFeePaymentReceivedChangeEvent :ChangeEventBase
{
    public DateTime PaidDate { get; private set; }
    public Money PaidFee { get; private set; }
    
    public ApplicationMembershipFeePaymentReceivedChangeEvent(DateTime paidDate, Money paidFee)
    {
        this.PaidDate = paidDate;
        this.PaidFee = paidFee;
    }
    
    private ApplicationMembershipFeePaymentReceivedChangeEvent(Guid aggregateId, long version, Guid? tenantId, DateTime paidDate, Money paidFee) : base(aggregateId, version, tenantId)
    {
        this.PaidDate = paidDate;
        this.PaidFee = paidFee;
    }
    
    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationMembershipFeePaymentReceivedChangeEvent(aggregateId, version, tenantId, this.PaidDate, this.PaidFee);
    }
}