using ExBot.Domain.Entities;
using ExBot.Domain.Repositories;
using ExBot.Infrastructure.Sql.Dto;
using ExBot.Infrastructure.Sql.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ObjectMapper;

namespace ExBot.Infrastructure.Sql;

/// <summary>
/// Dependency injection configuration for infrastructure layer
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add infrastructure services with database support
    /// </summary>
    public static IServiceCollection AddSqlData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Add SQL Server database context
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<SqlDbContext>(options => options.UseSqlServer(connectionString));
        }

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddMapping<User, UserDto>(true);

        return services;
    }

    public static async Task InitialiseSqlAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;

        //  ensure the database exists
        var sqlContext = provider.GetRequiredService<SqlDbContext>();
        await sqlContext.Database.MigrateAsync();  // creates DB + applies migrations
    }
}

