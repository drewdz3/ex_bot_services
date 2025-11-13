using ExBot.Application.DTOs;
using ExBot.Application.Interfaces;
using ExBot.Domain.Entities;
using ExBot.Domain.Repositories;

namespace ExBot.Application.Services;

/// <summary>
/// Implementation of TaskItem service
/// </summary>
public class TaskItemService : ITaskItemService
{
    private readonly ITaskItemRepository _taskItemRepository;

    public TaskItemService(ITaskItemRepository taskItemRepository)
    {
        _taskItemRepository = taskItemRepository;
    }

    public async Task<TaskItemDto?> GetTaskByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var task = await _taskItemRepository.GetByIdAsync(id, cancellationToken);
        return task == null ? null : MapToDto(task);
    }

    public async Task<IEnumerable<TaskItemDto>> GetAllTasksAsync(CancellationToken cancellationToken = default)
    {
        var tasks = await _taskItemRepository.GetAllAsync(cancellationToken);
        return tasks.Select(MapToDto);
    }

    public async Task<IEnumerable<TaskItemDto>> GetTasksByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var tasks = await _taskItemRepository.GetByUserIdAsync(userId, cancellationToken);
        return tasks.Select(MapToDto);
    }

    public async Task<TaskItemDto> CreateTaskAsync(CreateTaskItemRequest request, CancellationToken cancellationToken = default)
    {
        var taskItem = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            UserId = request.UserId,
            DueDate = request.DueDate,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        var createdTask = await _taskItemRepository.CreateAsync(taskItem, cancellationToken);
        return MapToDto(createdTask);
    }

    public async Task<TaskItemDto?> UpdateTaskAsync(Guid id, UpdateTaskItemRequest request, CancellationToken cancellationToken = default)
    {
        var taskItem = await _taskItemRepository.GetByIdAsync(id, cancellationToken);
        if (taskItem == null)
            return null;

        if (!string.IsNullOrWhiteSpace(request.Title))
            taskItem.Title = request.Title;

        if (!string.IsNullOrWhiteSpace(request.Description))
            taskItem.Description = request.Description;

        if (request.IsCompleted.HasValue)
            taskItem.IsCompleted = request.IsCompleted.Value;

        if (request.DueDate.HasValue)
            taskItem.DueDate = request.DueDate;

        taskItem.UpdatedAt = DateTime.UtcNow;

        var updatedTask = await _taskItemRepository.UpdateAsync(taskItem, cancellationToken);
        return MapToDto(updatedTask);
    }

    public async Task<bool> DeleteTaskAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _taskItemRepository.DeleteAsync(id, cancellationToken);
    }

    private static TaskItemDto MapToDto(TaskItem taskItem)
    {
        return new TaskItemDto
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            IsCompleted = taskItem.IsCompleted,
            UserId = taskItem.UserId,
            DueDate = taskItem.DueDate,
            CreatedAt = taskItem.CreatedAt
        };
    }
}
