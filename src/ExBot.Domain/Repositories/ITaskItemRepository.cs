using ExBot.Domain.Entities;

namespace ExBot.Domain.Repositories;

/// <summary>
/// Repository interface for TaskItem entity
/// </summary>
public interface ITaskItemRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<TaskItem> CreateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task<TaskItem> UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}
