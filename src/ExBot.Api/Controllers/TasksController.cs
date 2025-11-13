using ExBot.Application.DTOs;
using ExBot.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExBot.Api.Controllers;

/// <summary>
/// API controller for Task management operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class TasksController : ControllerBase
{
    private readonly ITaskItemService _taskItemService;
    private readonly ILogger<TasksController> _logger;

    public TasksController(ITaskItemService taskItemService, ILogger<TasksController> logger)
    {
        _taskItemService = taskItemService;
        _logger = logger;
    }

    /// <summary>
    /// Get all tasks
    /// </summary>
    /// <returns>List of all tasks</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TaskItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetAllTasks(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all tasks");
        var tasks = await _taskItemService.GetAllTasksAsync(cancellationToken);
        return Ok(tasks);
    }

    /// <summary>
    /// Get tasks by user ID
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of tasks for the user</returns>
    [HttpGet("user/{userId}")]
    [ProducesResponseType(typeof(IEnumerable<TaskItemDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TaskItemDto>>> GetTasksByUserId(Guid userId, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting tasks for user: {UserId}", userId);
        var tasks = await _taskItemService.GetTasksByUserIdAsync(userId, cancellationToken);
        return Ok(tasks);
    }

    /// <summary>
    /// Get a task by ID
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Task details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItemDto>> GetTaskById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting task with ID: {TaskId}", id);
        var task = await _taskItemService.GetTaskByIdAsync(id, cancellationToken);

        if (task == null)
        {
            _logger.LogWarning("Task not found with ID: {TaskId}", id);
            return NotFound();
        }

        return Ok(task);
    }

    /// <summary>
    /// Create a new task
    /// </summary>
    /// <param name="request">Task creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created task</returns>
    [HttpPost]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TaskItemDto>> CreateTask([FromBody] CreateTaskItemRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new task: {Title}", request.Title);
        var task = await _taskItemService.CreateTaskAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
    }

    /// <summary>
    /// Update an existing task
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <param name="request">Task update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated task</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(TaskItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TaskItemDto>> UpdateTask(Guid id, [FromBody] UpdateTaskItemRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating task with ID: {TaskId}", id);
        var task = await _taskItemService.UpdateTaskAsync(id, request, cancellationToken);

        if (task == null)
        {
            _logger.LogWarning("Task not found with ID: {TaskId}", id);
            return NotFound();
        }

        return Ok(task);
    }

    /// <summary>
    /// Delete a task
    /// </summary>
    /// <param name="id">Task ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTask(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting task with ID: {TaskId}", id);
        var result = await _taskItemService.DeleteTaskAsync(id, cancellationToken);

        if (!result)
        {
            _logger.LogWarning("Task not found with ID: {TaskId}", id);
            return NotFound();
        }

        return NoContent();
    }
}
