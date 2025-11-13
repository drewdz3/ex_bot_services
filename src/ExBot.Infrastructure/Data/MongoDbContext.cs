using MongoDB.Driver;

namespace ExBot.Infrastructure.Data;

/// <summary>
/// MongoDB context for document-based data (e.g., AI agent conversations)
/// </summary>
public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(string connectionString, string databaseName)
    {
        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    /// <summary>
    /// Collection for storing AI agent conversation history
    /// </summary>
    public IMongoCollection<AgentConversation> AgentConversations =>
        _database.GetCollection<AgentConversation>("agent_conversations");

    /// <summary>
    /// Collection for storing unstructured data and logs
    /// </summary>
    public IMongoCollection<DocumentLog> DocumentLogs =>
        _database.GetCollection<DocumentLog>("document_logs");
}

/// <summary>
/// Document model for AI agent conversations
/// </summary>
public class AgentConversation
{
    public string Id { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string AgentName { get; set; } = string.Empty;
    public List<ConversationMessage> Messages { get; set; } = new();
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Message within a conversation
/// </summary>
public class ConversationMessage
{
    public string Role { get; set; } = string.Empty; // "user", "assistant", "system"
    public string Content { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}

/// <summary>
/// Document model for general logging and unstructured data
/// </summary>
public class DocumentLog
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}
