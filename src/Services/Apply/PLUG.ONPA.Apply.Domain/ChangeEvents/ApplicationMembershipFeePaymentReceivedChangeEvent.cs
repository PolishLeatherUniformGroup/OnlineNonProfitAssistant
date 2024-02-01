using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public class ApplicationMembershipFeePaymentReceivedChangeEvent :ChangeEventBase
{
    public DateTime PaidDate { get; private set; }
    public Money PaidFee { get; private set; }
    
    public ApplicationMembershipFeePaymentReceivedChangeEvent(DateTime paidDate, Money paidFee)
    {
        PaidDate = paidDate;
        PaidFee = paidFee;
    }
    
    private ApplicationMembershipFeePaymentReceivedChangeEvent(Guid aggregateId, long version, Guid? tenantId, DateTime paidDate, Money paidFee) : base(aggregateId, version, tenantId)
    {
        PaidDate = paidDate;
        PaidFee = paidFee;
    }
    
    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationMembershipFeePaymentReceivedChangeEvent(aggregateId, version, tenantId, PaidDate, PaidFee);
    }
}