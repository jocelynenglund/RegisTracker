using Microsoft.EntityFrameworkCore;
using RegisTrackerSystem;
using Infrastructure;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register the AppDbContext with the dependency injection container
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IRepository<>), typeof(SQLRepository<>));
builder.Services.AddScoped<RegisTracker>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapPost("/registerInterest", async (RegisterInterestCommand command, RegisTracker registracker) =>
{
    try
    {
        await Task.Run(() => registracker.Handle(command)); // Add await to fix the async warning
        return Results.Ok("Interest registered successfully");
    }
    catch (InvalidOperationException e)
    {
        return Results.BadRequest(e.Message);
    }
})
.WithName("Register Interest");
app.MapGet("/test-connection", async (AppDbContext context) =>
{
    try
    {
        await context.Database.CanConnectAsync();
        return Results.Ok("Connection successful");
    }
    catch (Exception ex)
    {
        return Results.Problem("Connection failed: " + ex.Message);
    }
});

app.Run();

public partial class Program { }