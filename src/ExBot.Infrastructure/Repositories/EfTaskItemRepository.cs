using ExBot.Domain.Entities;
using ExBot.Domain.Repositories;
using ExBot.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ExBot.Infrastructure.Repositories;

/// <summary>
/// Entity Framework Core implementation of TaskItem repository
/// </summary>
public class EfTaskItemRepository : ITaskItemRepository
{
    private readonly ExBotDbContext _context;

    public EfTaskItemRepository(ExBotDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tasks.ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TaskItem>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await _context.Tasks
            .Where(t => t.UserId == userId)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaskItem> CreateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Add(taskItem);
        await _context.SaveChangesAsync(cancellationToken);
        return taskItem;
    }

    public async Task<TaskItem> UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
    {
        _context.Tasks.Update(taskItem);
        await _context.SaveChangesAsync(cancellationToken);
        return taskItem;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task = await GetByIdAsync(id, cancellationToken);
        if (task == null)
            return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
