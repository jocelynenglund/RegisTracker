
namespace RegisTrackerSystem.Domain;

public interface AggregateRoot
{
    Guid Id { get; }
    IReadOnlyCollection<DomainEvent> UncommittedEvents { get; }
    void Apply(DomainEvent e);
    void ClearUncommittedEvents();
}

public interface ReadModel
{
    Guid Id { get; }
}