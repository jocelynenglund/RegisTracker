using RegisTrackerSystem.Domain;

namespace RegisTrackerSystem;

public interface IRepository<T> where T : class, AggregateRoot
{
    Task Save(T entity);
    IEnumerable<T> GetAll();
}
public interface IReadModelRepository<T> where T: class, ReadModel
{
    Task Save(T entity);
    IEnumerable<T> GetAll();
}