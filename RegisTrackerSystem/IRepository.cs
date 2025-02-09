namespace RegisTrackerSystem;

public interface IRepository<T> where T : class
{
    void Save(T entity);
    IEnumerable<T> GetAll();
}
