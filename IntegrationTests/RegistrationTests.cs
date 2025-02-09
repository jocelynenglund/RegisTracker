using Bogus;
using FastEndpoints;
using Microsoft.AspNetCore.Mvc.Testing;
using RegisTrackerSystem;
using Shouldly;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks.Dataflow;
using WebApi.Endpoints;

namespace IntegrationTests;
public class RegistrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Faker _faker = new();
    public RegistrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task WhenRegisteringInterest_ShouldReturnOk()
    {
        var email = _faker.Internet.Email();
        var response = await _client.PostAsJsonAsync(RegisterInterestEndpoint.RegisterInterestEndpointUrl,
            new RegisterInterestCommand(email, 2000));
        
        var message = await response.Content.ReadFromJsonAsync<string>();

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        message.ShouldBe(RegisTracker.RegistrationSuccess);
    }

    [Fact]
    public async Task WhenRegisteringTwice_ShouldReturnBadRequest()
    {
        var email = _faker.Internet.Email();
        var response = await _client.PostAsJsonAsync(RegisterInterestEndpoint.RegisterInterestEndpointUrl,
            new RegisterInterestCommand(email, 2000));
       
        response = await _client.PostAsJsonAsync(RegisterInterestEndpoint.RegisterInterestEndpointUrl,
            new RegisterInterestCommand(email, 2000));
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>();


        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        problemDetails.Errors.First().Reason.ShouldBe(RegisTracker.RegistrationErrorExisting);
    }
}
