using ExBot.Application.Interfaces;
using ExBot.Application.Services;
using ExBot.Domain.Repositories;
using ExBot.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ExBot.Infrastructure;

/// <summary>
/// Dependency injection configuration for infrastructure layer
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // Register repositories
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
