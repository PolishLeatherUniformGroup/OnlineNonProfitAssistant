namespace PLUG.ONPA.Apply.Read.Models;

public sealed class Application
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public Address Address { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public DateTime ApplicationDate { get; set; }
    public int Status { get; set; }
    public List<Recommendation> Recommendations { get; set; }
}