using RegisTrackerSystem;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register FakeRepository for all IRepository<T> instances
builder.Services.AddScoped(typeof(IRepository<>), typeof(FakeRepository<>));
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
        registracker.Handle(command);
        return Results.Ok("Interest registered successfully");
    }
    catch (InvalidOperationException e)
    {
        return Results.BadRequest(e.Message);
    }
})
.WithName("Register Interest");

app.Run();

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

public partial class Program { }