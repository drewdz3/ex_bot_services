using ExBot.Application.Interfaces;
using ExBot.Application.Services;
using ExBot.Domain.Repositories;
using ExBot.Infrastructure.Data;
using ExBot.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExBot.Infrastructure;

/// <summary>
/// Dependency injection configuration for infrastructure layer
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add infrastructure services with database support
    /// </summary>
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        bool useDatabase = true)
    {
        if (useDatabase)
        {
            // Add SQL Server database context
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (!string.IsNullOrEmpty(connectionString))
            {
                services.AddDbContext<ExBotDbContext>(options =>
                    options.UseSqlServer(connectionString));

                // Register EF Core repositories
                services.AddScoped<IUserRepository, EfUserRepository>();
                services.AddScoped<ITaskItemRepository, EfTaskItemRepository>();
            }
            else
            {
                // Fallback to in-memory if no connection string
                AddInMemoryRepositories(services);
            }

            // Add MongoDB context
            var mongoConnectionString = configuration.GetConnectionString("MongoDbConnection");
            var mongoDatabaseName = configuration["MongoDb:DatabaseName"] ?? "ExBotDb";
            if (!string.IsNullOrEmpty(mongoConnectionString))
            {
                services.AddSingleton<MongoDbContext>(sp =>
                    new MongoDbContext(mongoConnectionString, mongoDatabaseName));
            }
        }
        else
        {
            // Use in-memory repositories for testing/development
            AddInMemoryRepositories(services);
        }

        return services;
    }

    /// <summary>
    /// Add infrastructure services with in-memory storage (backwards compatible)
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return AddInMemoryRepositories(services);
    }

    private static IServiceCollection AddInMemoryRepositories(IServiceCollection services)
    {
        services.AddSingleton<IUserRepository, InMemoryUserRepository>();
        services.AddSingleton<ITaskItemRepository, InMemoryTaskItemRepository>();
        return services;
    }

    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register application services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ITaskItemService, TaskItemService>();

        return services;
    }
}

