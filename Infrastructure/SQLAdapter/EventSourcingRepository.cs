using Infrastructure.SQLAdapter.Migrations;
using Microsoft.EntityFrameworkCore;
using RegisTrackerSystem;
using RegisTrackerSystem.Domain;
using System.Text.Json;

namespace Infrastructure.SQLAdapter;
public class EventSourcingRepository<T> : IRepository<T> where T : class, AggregateRoot, new()
{
    private readonly AppDbContext _context;
    private readonly DbSet<EventEntity> _eventStore;
    public EventSourcingRepository(AppDbContext context)
    {
        _context = context;
        _eventStore = _context.EventStore;
    }
    public IEnumerable<T> GetAll()
    {
        var aggregates = new List<T>();
        var aggregateIds = _eventStore.Select(e => e.AggregateId).Distinct().ToList();

        foreach (var id in aggregateIds)
        {
            var events = _eventStore.Where(e => e.AggregateId == id).OrderBy(e => e.TimeStamp).ToList();
            var aggregate = new T();
            foreach (var @event in events)
            {
                var domainEvent = (DomainEvent)JsonSerializer.Deserialize(@event.EventData, Type.GetType(@event.EventType));
                aggregate.Apply(domainEvent);
            }
            aggregates.Add(aggregate);
        }

        return aggregates;
    }

    public async Task Save(T aggregate)
    {
        var uncommittedEvents = aggregate.UncommittedEvents;
        foreach (var @event in uncommittedEvents)
        {
            var eventEntity = new EventEntity(@event, aggregate.Id);
            _eventStore.Add(eventEntity);
        }
        aggregate.ClearUncommittedEvents();
        await _context.SaveChangesAsync();
    }
}
