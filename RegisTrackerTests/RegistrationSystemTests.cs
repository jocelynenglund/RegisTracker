using FluentAssertions;
using RegisTrackerSystem;

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
    private readonly RegisTracker sut;
    public RegistrationSystemTests()
    {
        sut = new(fakeIndividualRepository);
    }

    [Fact]
    public void WhenRegisterInterestCommandIsTriggeredOnce_StatisticsAreReflected()
    {
        var registerInterestCommand = new RegisterInterestCommand("aperson@example.com", 2000);

        sut.Handle(registerInterestCommand);
        var result = sut.GetStatistics();


        result.Batches.Should().ContainSingle();
        result.Batches.First(x=>x.Year == 2000).Count.Should().Be(1);
    }

    [Theory]
    [InlineData(2)]
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

}
public class FakeRepository<T> : IRepository<T> where T : class
{
    public List<T> Entities { get; } = new();
    public void Save(T entity)
    {
        Entities.Add(entity);
    }
    public IEnumerable<T> GetAll()
    {
        return Entities;
    }
}   
