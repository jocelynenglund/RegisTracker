using RegisTrackerSystem;
using RegisTrackerSystem.Domain;
using Shouldly;

namespace RegisTrackerTests;
public class IndividualTests
{

    [Fact]
    public void WhenSubmittingInterest_SetsEmailAndStatus()
    {
        var individual = new Individual();
        var cmd = new RegisterInterestCommand("a@person.com", 2000);

        individual.RegisterInterest(cmd.Email, cmd.Year);

        individual.Email.ShouldBe(cmd.Email);
        individual.Status.ShouldBe(Status.InterestSubmitted);
    }

    [Fact]
    public void WhenConfirmRegistration_ShouldChangeStatus()
    {
        var previousEvents = new List<DomainEvent>
        {
            new InterestRegistered("a@example.com", 2000)
        };

        var individual = new Individual(previousEvents);

        individual.ConfirmRegistration();

        individual.Status.ShouldBe(Status.RegistrationConfirmed);
    }

    [Fact]
    public void WhenConfirmRegistrationTwice_ShouldThrowException()
    {
        var previousEvents = new List<DomainEvent>
        {
            new InterestRegistered("a@example.com", 2000),
            new RegistrationConfirmed("a@example.com")
        };

        var individual = new Individual(previousEvents);

        var exception = Should.Throw<InvalidOperationException>(() => individual.ConfirmRegistration());
        exception.Message.ShouldBe("Email already confirmed");

    }

    [Fact]
    public void WhenConfirmRegistrationWithoutInterestSubmitted_ShouldThrowException()
    {
        var previousEvents = new List<DomainEvent>
        {
        
        };

        var individual = new Individual(previousEvents);

        var exception = Should.Throw<InvalidOperationException>(() => individual.ConfirmRegistration());
        exception.Message.ShouldBe("Email not registered");

    }
}
 