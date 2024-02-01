using System.Text.Json;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Common.Infrastructure.Storage;

public class DomainEventEntry
{
    public DomainEventEntry(Guid id, Type eventType, string eventTypeName, string data, ProcessingStatus status)
    {
        this.Id = id;
        this.EventType = eventType;
        this.EventTypeName = eventTypeName;
        this.Data = data;
        this.Status = status;
    }

    public Guid Id { get; set; }
    public Type EventType { get; set; }
    public string EventTypeName { get; set; }
    public string Data { get; set; }
    public ProcessingStatus Status { get; set; }
    
    public static DomainEventEntry WithData<T>(T domainEvent) where T : IDomainEvent
    {
        return new DomainEventEntry(
            domainEvent.EventId,
            domainEvent.GetType(),
            domainEvent.GetType().Name,
            JsonSerializer.Serialize(domainEvent),
            ProcessingStatus.NotStarted
        );
    }
}