using PLUG.ONPA.Apply.Domain.ChangeEvents;

namespace PLUG.ONPA.Apply.Domain.Model;

public partial class ApplicationForm
{
    public void ApplyChange(ApplicationFormCreated @event)
    {
        this.FirstName = @event.FirstName;
        this.LastName = @event.LastName;
        this.Address = @event.Address;
        this.BirthDate = @event.BirthDate;
        this.Email = @event.Email;
        this.PhoneNumber = @event.PhoneNumber;
        this.Status = @event.Status;
        this.ApplicationDate = @event.ApplicationDate;
        foreach (var recommendation in @event.Recommendations)
        {
            this.recommendations.Add(recommendation);
        }
    }
}