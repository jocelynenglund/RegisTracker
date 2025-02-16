using RegisTrackerSystem.Domain;
using System.Text.Json;

namespace Infrastructure.SQLAdapter;

public record EventEntity
{
    public EventEntity() { }
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid AggregateId { get; set; }
    public string EventType { get; set; } 
    public string EventData { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
    public EventEntity(DomainEvent domainEvent, Guid aggregateId)
    {
        EventType = domainEvent.GetType().AssemblyQualifiedName;
        AggregateId = aggregateId;
        EventData = JsonSerializer.Serialize((dynamic)domainEvent);
        TimeStamp = domainEvent.TimeStamp;
    }
}
