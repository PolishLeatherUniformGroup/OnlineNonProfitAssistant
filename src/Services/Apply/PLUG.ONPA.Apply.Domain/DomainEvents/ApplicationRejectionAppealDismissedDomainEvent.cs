using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public class ApplicationRejectionAppealDismissedDomainEvent : DomainEventBase
{
    public NonEmptyString Email { get; private set; }
    public DateTime AppealDeadline { get; private set; }
    public DateTime AppealDate { get; private set; }

    public ApplicationRejectionAppealDismissedDomainEvent(NonEmptyString email, DateTime appealDeadline,
        DateTime appealDate)
    {
        this.Email = email;
        this.AppealDeadline = appealDeadline;
        this.AppealDate = appealDate;
    }

    private ApplicationRejectionAppealDismissedDomainEvent(Guid aggregateId, Guid? tenantId, NonEmptyString email,
        DateTime appealDeadline, DateTime appealDate) : base(aggregateId, tenantId)
    {
        this.Email = email;
        this.AppealDeadline = appealDeadline;
        this.AppealDate = appealDate;
    }

    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationRejectionAppealDismissedDomainEvent(aggregateId, tenantId, this.Email,
            this.AppealDeadline, this.AppealDate);
    }
}