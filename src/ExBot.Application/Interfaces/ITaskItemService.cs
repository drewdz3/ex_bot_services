using ExBot.Application.DTOs;

namespace ExBot.Application.Interfaces;

/// <summary>
/// Service interface for TaskItem operations
/// </summary>
public interface ITaskItemService
{
    Task<TaskItemDto?> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskItemDto>> GetAllTasksAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskItemDto>> GetTasksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<TaskItemDto> CreateTaskAsync(CreateTaskItemRequest request, CancellationToken cancellationToken = default);
    Task<TaskItemDto?> UpdateTaskAsync(Guid id, UpdateTaskItemRequest request, CancellationToken cancellationToken = default);
    Task<bool> DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default);
}
