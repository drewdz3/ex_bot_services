using System.ComponentModel.DataAnnotations.Schema;

namespace ExBot.Infrastructure.Cosmos.Dto
{
    [Table("ConversationMessage")]
    public class ConversationMessageDto
    {
        public string Role { get; set; } = string.Empty; // "user", "assistant", "system"
        public string Content { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
