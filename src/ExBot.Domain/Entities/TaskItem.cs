using ExBot.Domain.Common;

namespace ExBot.Domain.Entities;

/// <summary>
/// Task entity representing a user task
/// </summary>
public class TaskItem : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsCompleted { get; set; } = false;
    public Guid UserId { get; set; }
    public DateTime? DueDate { get; set; }
}
