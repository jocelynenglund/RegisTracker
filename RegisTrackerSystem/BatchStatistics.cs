namespace RegisTrackerSystem;

public class BatchStatistics
{
    public BatchStatistics(int year, int count)
    {
        Year = year;
        Count = count;
    }
    public int Year { get; }
    public int Count { get; private set; }
    public void Increment() => Count++;
}
