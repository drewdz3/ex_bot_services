namespace ExBot.Domain.Entities;

public class DocumentLog
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, object> Data { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}