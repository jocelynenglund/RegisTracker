using RegisTrackerSystem.Domain;

namespace RegisTrackerSystem;
public record RegisterInterestCommand(string Email, int Year);
public class RegisTracker(IRepository<Individual> individuals, IRepository<BatchStatistics> statistics)
{
    public const string RegistrationSuccess  = "Registration created successfully";
    public const string RegistrationErrorExisting = "That email is already registered";
    public RegistrationStatistics GetStatistics()
    {
        var all = statistics.GetAll();
        var total = all.Sum(x => x.Count);
        return new RegistrationStatistics(total, all.ToDictionary(x=>x.Year, y=>y.Count));
    }

    public async Task Handle(RegisterInterestCommand interest)
    {
        if (individuals.GetAll().Any(x => x.Email == interest.Email))
        {
            throw new InvalidOperationException(RegistrationErrorExisting);
        }
        await individuals.Save(new Individual()
        {
            Email = interest.Email,
        });

        var statistic = statistics.GetAll().FirstOrDefault(x => x.Year ==  interest.Year) 
            ?? new BatchStatistics(interest.Year, 0);
        statistic.Increment();
        statistics.Save(statistic);
    }
}
