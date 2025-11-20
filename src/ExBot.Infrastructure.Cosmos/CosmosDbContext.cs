using ExBot.Infrastructure.Cosmos.Dto;
using Microsoft.EntityFrameworkCore;

namespace ExBot.Infrastructure.Cosmos;

/// <summary>
/// Cosmos DB context for document-based data (e.g., AI agent conversations)
/// </summary>
public class CosmosDbContext : DbContext
{
    public CosmosDbContext(DbContextOptions<CosmosDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AgentConversationDto>().ToContainer(ToContainerName(nameof(AgentConversationDto)));
        modelBuilder.Entity<DocumentLogDto>().ToContainer(ToContainerName(nameof(DocumentLogDto)));
    }

    private string ToContainerName(string name)
    {
        if (name.EndsWith("Dto"))
        {
            name = name[..^3];
        }
        return name;
    }
}

