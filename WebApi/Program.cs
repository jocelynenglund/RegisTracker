using FastEndpoints;
using Infrastructure;
using Infrastructure.SQLAdapter.Migrations;
using RegisTrackerSystem;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCoreServices(builder.Configuration);
builder.Services.AddFastEndpoints();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.UseFastEndpoints();
app.Run();

public partial class Program { }