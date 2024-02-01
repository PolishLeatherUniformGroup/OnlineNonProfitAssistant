using PLUG.ONPA.Apply.Domain.Model;
using PLUG.ONPA.Common.Domain;
using PLUG.ONPA.Common.Domain.Abstractions;
using PLUG.ONPA.Common.Models;

namespace PLUG.ONPA.Apply.Domain.ChangeEvents;

public class ApplicationFormCreatedChangeEvent : ChangeEventBase
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Address Address { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public string Email { get; private set; }
    public string PhoneNumber { get; private set; }
    public ApplicationStatus Status { get; private set; }
    public DateTime ApplicationDate { get; private set; }
    public List<ApplicationRecommendation> Recommendations { get; private set; }

    public ApplicationFormCreatedChangeEvent(string firstName, string lastName, Address address, DateOnly birthDate, string email, string phoneNumber, ApplicationStatus status, DateTime applicationDate, List<ApplicationRecommendation> recommendations)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Address = address;
        this.BirthDate = birthDate;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.Status = status;
        this.ApplicationDate = applicationDate;
        this.Recommendations = recommendations;
    }

    private ApplicationFormCreatedChangeEvent(Guid aggregateId, long version, Guid? tenantId, string firstName, string lastName, Address address, DateOnly birthDate, string email, string phoneNumber, ApplicationStatus status, DateTime applicationDate, List<ApplicationRecommendation> recommendations) : base(aggregateId, version, tenantId)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
        this.Address = address;
        this.BirthDate = birthDate;
        this.Email = email;
        this.PhoneNumber = phoneNumber;
        this.Status = status;
        this.ApplicationDate = applicationDate;
        this.Recommendations = recommendations;
    }


    public override IChangeEvent WithAggregate(Guid aggregateId, long version, Type aggregateType, Guid? tenantId = null)
    {
        return new ApplicationFormCreatedChangeEvent(aggregateId, version, tenantId, this.FirstName, this.LastName, this.Address, this.BirthDate, this.Email, this.PhoneNumber, this.Status, this.ApplicationDate, this.Recommendations);
    }
}