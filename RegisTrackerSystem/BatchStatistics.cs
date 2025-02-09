namespace RegisTrackerSystem;
public class BatchStatistics: AggregateRoot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public BatchStatistics()
    {
        
    }
    public BatchStatistics(int year, int count)
    {
        Year = year;
        Count = count;
    }
    public int Year { get; set; }
    public int Count { get; private set; }
    public void Increment() => Count++;
}
