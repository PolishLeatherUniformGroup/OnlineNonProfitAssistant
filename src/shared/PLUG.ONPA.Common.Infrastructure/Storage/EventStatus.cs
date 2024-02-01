namespace PLUG.ONPA.Common.Infrastructure.Storage;

public class EventStatus
{
    public Guid EventId { get; set; }
    public ProcessingStatus Status { get; set; }

    public EventStatus()
    {
        
    }
    public EventStatus(EventEntry eventEntry)
    {
        this.EventId = eventEntry.Id;
        this.Status = ProcessingStatus.NotStarted;
    }
}

public enum ProcessingStatus
{
    NotStarted,
    InProgress,
    Completed
}