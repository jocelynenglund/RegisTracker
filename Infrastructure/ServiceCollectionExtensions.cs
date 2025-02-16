using Infrastructure.SQLAdapter;
using Infrastructure.SQLAdapter.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RegisTrackerSystem;
using RegisTrackerSystem.Domain;
using System.Reflection;

namespace Infrastructure;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        services.AddScoped(typeof(IReadModelRepository<>), typeof(SQLRepository<>));
        services.AddScoped(typeof(IRepository<>), typeof(EventSourcingRepository<>));
     

        return services;
    }
}
