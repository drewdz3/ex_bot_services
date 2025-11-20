using ExBot.Domain.Entities;
using ExBot.Infrastructure.Cosmos.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ObjectMapper;

namespace ExBot.Infrastructure.Cosmos;

/// <summary>
/// Dependency injection configuration for infrastructure layer
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Add infrastructure services with database support
    /// </summary>
    /// </summary>
    public static IServiceCollection AddCosmosData(
        this IServiceCollection services,
        IConfiguration configuration)
    {
            services.AddDbContext<CosmosDbContext>(options =>
                options.UseCosmos(
                    configuration["Cosmos:Endpoint"] ?? String.Empty,
                    configuration["Cosmos:Key"] ?? String.Empty,
                    configuration["Cosmos:DatabaseName"] ?? String.Empty
                ));

        //  add repos

        //  add conversions
        services.AddMapping<AgentConversation, AgentConversationDto>(true);
        services.AddMapping<ConversationMessage, ConversationMessageDto>(true);
        services.AddMapping<DocumentLog, DocumentLogDto>(true);

        return services;
    }

    public static async Task InitialiseCosmosAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var provider = scope.ServiceProvider;

        //  ensure the database exists
        var cosmosContext = provider.GetRequiredService<CosmosDbContext>();
        await cosmosContext.Database.EnsureCreatedAsync(); // creates DB + containers
    }
}

