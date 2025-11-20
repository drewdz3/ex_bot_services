using System.ComponentModel.DataAnnotations.Schema;

namespace ExBot.Infrastructure.Cosmos.Dto
{
    [Table("DocumentLog")]
    public class DocumentLogDto
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public Dictionary<string, object> Data { get; set; } = new();
        public DateTime CreatedAt { get; set; }
    }
}
