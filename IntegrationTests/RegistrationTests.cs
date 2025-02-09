using Microsoft.AspNetCore.Mvc.Testing;
using RegisTrackerSystem;
using Shouldly;
using System.Net;
using System.Net.Http.Json;

namespace IntegrationTests;
public class RegistrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public RegistrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task WhenRegisteringInterest_ShouldReturnOk()
    {
        var response = await _client.PostAsJsonAsync("/registerInterest",
            new RegisterInterestCommand("jocelyn@englund.com", 2000));

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Fact]
    public async Task WhenRegisteringTwice_ShouldReturnBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/registerInterest",
            new RegisterInterestCommand("jocelyn@englund.com", 2000));
       
        response = await _client.PostAsJsonAsync("/registerInterest",
            new RegisterInterestCommand("jocelyn@englund.com", 2000));

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}
