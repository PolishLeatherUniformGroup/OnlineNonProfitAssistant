using System.Text.Json;
using PLUG.ONPA.Common.Domain.Abstractions;

namespace PLUG.ONPA.Common.Infrastructure.Storage;
    
public sealed class EventEntry 
{
    public Guid Id { get; set; }
    public long Version { get; set; }
    public string Data { get; set; }
    public Guid StreamId { get; set; }
    public string StreamName { get; set; }
    public Type StreamType { get; set; }
    public Guid? TenantId { get; set; }
    public Type EventType { get; set; }
    public string TypeName { get; set; }
    
    public static EventEntry  WithData<T>(T eventData) where T : IChangeEvent
    {
        return new EventEntry()
        {
            Id = eventData.EventId,
            Version = eventData.Version,
            Data = JsonSerializer.Serialize(eventData),
            StreamId = eventData.AggregateId,
            TenantId = eventData.TenantId,
            EventType = eventData.GetType(),
            TypeName = eventData.GetType().Name,
            StreamName = eventData.AggregateType.Name,
            StreamType = eventData.AggregateType
        };
    }
}

public static class EventEntryExtensions
{
    
    public static T? GetData<T>(this EventEntry eventEntry) where T : IChangeEvent
    {
        return JsonSerializer.Deserialize<T>(eventEntry.Data);
    }
}