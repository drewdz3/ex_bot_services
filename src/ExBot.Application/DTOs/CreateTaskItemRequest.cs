namespace ExBot.Application.DTOs;

/// <summary>
/// Request DTO for creating a new task
/// </summary>
public class CreateTaskItemRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public DateTime? DueDate { get; set; }
}
