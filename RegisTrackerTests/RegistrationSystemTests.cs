using FluentAssertions;
using RegisTrackerSystem;
using RegisTrackerSystem.Domain;

namespace RegisTrackerTests;

/// <summary>
/// Test list:
/// 
/// When interest is registered, there should be 1 entry in the read model with a count of one 
/// When an interest is registered for a specific year, the statistics should reflect that
/// an email cannot be registered more than once
/// </summary>
/// 


public class RegistrationSystemTests
{
    private readonly FakeRepository<Individual> fakeIndividualRepository = new();
    private readonly FakeRepository<BatchStatistics> fakeBatchStatisticsRepository = new();
    private readonly RegisTracker sut;
    public RegistrationSystemTests()
    {
        sut = new(fakeIndividualRepository, fakeBatchStatisticsRepository);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void WhenRegisterInterestCommandIsTriggered_StatisticsAreReflected(int repetitions)
    {
        for (int i = 0; i < repetitions; i++)
        {
            var registerInterestCommand = new RegisterInterestCommand($"aperson{i}@example.com", 2000);
            sut.Handle(registerInterestCommand);
        }

        var result = sut.GetStatistics();


        result.BatchCount[2000].Should().Be(repetitions);
    }

    [Theory]
    [InlineData(2)]
    [InlineData(3)]
    public void WhenRegisteringInterestMoreThanOnce_ThrowsException(int repetitions)
    {
        var registerInterestCommand = new RegisterInterestCommand("aperson@example.com", 2000);
        sut.Handle(registerInterestCommand);
        for (int i = 0; i < repetitions; i++)
        {
            Action act = () => sut.Handle(registerInterestCommand);
            act.Should().Throw<InvalidOperationException>()
                .WithMessage("That email is already registered",
                because: "because repeat submissions are not allowed");
        }
    }

    [Fact]
    public void WhenInterestIsRegisteredForASpecificYear_StatisticsShouldReflectThat()
    {
        var registerInterestCommand = new RegisterInterestCommand("person@example.com", 2000);
        sut.Handle(registerInterestCommand);
        registerInterestCommand = new RegisterInterestCommand("anotherperson@example.com", 2001);
        sut.Handle(registerInterestCommand);

        var statistics = sut.GetStatistics();

        statistics.BatchCount[2000].Should().Be(1);
        statistics.BatchCount[2001].Should().Be(1);
        statistics.Total.Should().Be(2);

    }

}
public class FakeRepository<T> : IRepository<T> where T : class
{
    public List<T> Entities { get; } = new();
    public void Save(T entity)
    {
        if (!Entities.Contains(entity))
        {
            Entities.Add(entity);
        }
    }
    public IEnumerable<T> GetAll()
    {
        return Entities;
    }
}
