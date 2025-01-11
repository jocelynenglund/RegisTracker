namespace RegisTrackerSystem;
public record RegisterInterestCommand(string Email, int Year);
public record RegistrationStatistics(int Total, IEnumerable<BatchStatistics> Batches);
public record BatchStatistics(int Year, int Count);

internal class RegisTracker(IRepository<Individual> individuals)
{
    public RegistrationStatistics GetStatistics()
    {
        return new RegistrationStatistics(0, new List<BatchStatistics>() { new BatchStatistics(2000, 1) });
    }

    public void Handle(RegisterInterestCommand interest)
    {
        if (individuals.GetAll().Any(x => x.Email == interest.Email))
        {
            throw new InvalidOperationException("That email is already registered");
        }
        individuals.Save(new Individual()
        {
            Email = interest.Email,
        });
    }
}

internal record Individual()
{
    public string Email { get; internal set; }
}
internal interface IRepository<T> where T : class
{
    void Save(T entity);
    IEnumerable<T> GetAll();
}
