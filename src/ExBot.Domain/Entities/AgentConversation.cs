namespace ExBot.Domain.Entities;

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