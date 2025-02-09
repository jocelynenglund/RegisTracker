namespace RegisTrackerSystem;

public interface IRepository<T> where T : class
{
    Task Save(T entity);
    IEnumerable<T> GetAll();
}
