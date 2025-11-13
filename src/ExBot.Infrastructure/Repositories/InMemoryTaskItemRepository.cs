using ExBot.Domain.Entities;
using ExBot.Domain.Repositories;

namespace ExBot.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of TaskItem repository for demonstration
/// In production, this would be replaced with actual database implementation
/// </summary>
public class InMemoryTaskItemRepository : ITaskItemRepository
{
    private readonly List<TaskItem> _tasks = new();

    public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        return Task.FromResult(task);
    }

    public Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<TaskItem>>(_tasks);
    }

    public Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tasks = _tasks.Where(t => t.UserId == userId);
        return Task.FromResult<IEnumerable<TaskItem>>(tasks);
    }

    public Task<TaskItem> CreateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _tasks.Add(taskItem);
        return Task.FromResult(taskItem);
    }

    public Task<TaskItem> UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        var existingTask = _tasks.FirstOrDefault(t => t.Id == taskItem.Id);
        if (existingTask != null)
        {
            _tasks.Remove(existingTask);
            _tasks.Add(taskItem);
        }
        return Task.FromResult(taskItem);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            _tasks.Remove(task);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
