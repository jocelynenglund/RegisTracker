namespace RegisTrackerSystem;

public class Individual: AggregateRoot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Individual()
    {
        
    }
    public Individual(string Email, Status Status): this()
    {
        this.Email = Email;
        this.Status = Status;
    }
    public Individual(List<DomainEvent> previousEvents) : this()
    {
        foreach (dynamic @event in previousEvents)
        {
            Apply(@event);
        }
    }

    public string Email { get; internal set; } 
    public Status Status { get; internal set; }

    internal void RegisterInterest(string email, int year)
    {
        Apply(new InterestRegistered(email, year));
    }

    internal void ConfirmRegistration()
    {
        if (Status != Status.InterestSubmitted)
        {
            throw new InvalidOperationException(Status >= Status.RegistrationConfirmed 
                ? "Email already confirmed"
                :"Email not registered");
        }
        Apply(new RegistrationConfirmed(Email));
    }

    private void Apply(InterestRegistered interestRegistered)
    {
        Email = interestRegistered.Email;
        Status = Status.InterestSubmitted;
    }
    private void Apply(RegistrationConfirmed registrationConfirmed)
    {
        Status = Status.RegistrationConfirmed;
    }

}
public abstract record DomainEvent()
{
    public DateTime TimeStamp { get; } = DateTime.UtcNow;
}
public record RegistrationConfirmed(string Email) : DomainEvent;
public record InterestRegistered(string Email, int Year) : DomainEvent;
public enum Status
{
    None,
    InterestSubmitted,
    RegistrationConfirmed
}