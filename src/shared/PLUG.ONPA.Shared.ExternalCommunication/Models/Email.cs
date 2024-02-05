namespace PLUG.ONPA.Shared.ExternalCommunication.Models;

public sealed class Email
{
    public Guid ThreadId { get; set; }
    public string Subject { get; set; }
    public string? From { get; set; }
    public string? To { get; set; }
    public string Body { get; set; }
    public List<string> Cc { get; set; }
    public DateTime SentDate { get; set; }
    
}