using FastEndpoints;
using Microsoft.AspNetCore.Http.HttpResults;
using RegisTrackerSystem;

namespace WebApi.Endpoints;

public record RegisterInterestRequest(string Email, int Year);
public class RegisterInterestEndpoint(RegisTracker _registracker) :
    Endpoint<RegisterInterestRequest,
    Results<Ok<string>,
        ProblemDetails>>
{
    public const string RegisterInterestEndpointUrl = "/registerInterest";

    public override void Configure()
    {
        Post(RegisterInterestEndpointUrl);
        AllowAnonymous();
    }
    public override async Task HandleAsync(RegisterInterestRequest req, CancellationToken ct)
    {
        try
        {
            var command = new RegisterInterestCommand(req.Email, req.Year);
            await _registracker.Handle(command);
            
            await SendResultAsync(TypedResults.Ok(RegisTracker.RegistrationSuccess));
        }
        catch (InvalidOperationException e)
        {
            AddError(r=>r.Email, e.Message);
            await SendResultAsync(TypedResults.BadRequest(new ProblemDetails(ValidationFailures)));
        }
    }
}
