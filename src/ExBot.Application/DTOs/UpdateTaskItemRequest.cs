namespace ExBot.Application.DTOs;

/// <summary>
/// Request DTO for updating a task
/// </summary>
public class UpdateTaskItemRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? IsCompleted { get; set; }
    public DateTime? DueDate { get; set; }
}
