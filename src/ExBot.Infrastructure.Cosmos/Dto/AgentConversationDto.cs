using Microsoft.EntityFrameworkCore;

namespace ExBot.Infrastructure.Cosmos.Dto
{
    public class AgentConversationDto
    {
        public string Id { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public string AgentName { get; set; } = string.Empty;
        public List<ConversationMessageDto> Messages { get; set; } = new();
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
