using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.DomainEvents;

public class ApplicationFormReceivedDomainEvent : DomainEventBase
{
    public NonEmptyString FirstName { get; private set; }
    public NonEmptyString LastName { get; private set; }
    public Address Address { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public NonEmptyString Email { get; private set; }
    public NonEmptyString PhoneNumber { get; private set; }
    public List<ApplicationRecommendation> Recommendations { get; private set; }
    public DateTime ApplicationDate { get; private set; }

    public ApplicationFormReceivedDomainEvent(NonEmptyString firstName, NonEmptyString lastName, Address address, DateOnly birthDate, NonEmptyString email, NonEmptyString phoneNumber, List<ApplicationRecommendation> recommendations, DateTime applicationDate)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Address = address;
        this.BirthDate = birthDate;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.Recommendations = recommendations;
        this.ApplicationDate = applicationDate;
    }

    private ApplicationFormReceivedDomainEvent(Guid aggregateId, Guid? tenantId, NonEmptyString firstName, NonEmptyString lastName, Address address, DateOnly birthDate, NonEmptyString email, NonEmptyString phoneNumber, List<ApplicationRecommendation> recommendations, DateTime applicationDate) : base(aggregateId, tenantId)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Address = address;
        this.BirthDate = birthDate;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.Recommendations = recommendations;
        this.ApplicationDate = applicationDate;
    }

    public override IDomainEvent WithAggregate(Guid aggregateId, Guid? tenantId)
    {
        return new ApplicationFormReceivedDomainEvent(aggregateId,
            tenantId,
            this.FirstName,
            this.LastName,
            this.Address,
            this.BirthDate,
            this.Email,
            this.PhoneNumber,
            this.Recommendations,
            this.ApplicationDate);
    }
}