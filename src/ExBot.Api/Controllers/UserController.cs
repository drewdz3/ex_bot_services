using ExBot.Application.UseCases;
using ExBot.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace ExBot.Api.Controllers;

/// <summary>
/// API controller for User management operations
/// Requires Azure Entra ID authentication
/// </summary>
[Authorize]
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[RequiredScope("access_as_user")]
public class UserController : ControllerBase
{
    #region Fields

    private readonly ILogger<UserController> _logger;
    private readonly IGetUserUc _GetUserUc;
    private readonly IGetUsersUc _GetUsersUc;
    private readonly IUpdateUserUc _UpdateUserUc;
    private readonly IDeleteUserUc _DeleteUserUc;
    private readonly ICreateUserUc _CreateUserUc;

    #endregion Fields

    #region Constructors

    public UserController(ILogger<UserController> logger, IGetUserUc getUserUc, IGetUsersUc getUsersUc, IUpdateUserUc updateUserUc, IDeleteUserUc deleteUserUc, ICreateUserUc createUserUc)
    {
        _logger = logger;
        _GetUserUc = getUserUc;
        _GetUsersUc = getUsersUc;
        _UpdateUserUc = updateUserUc;
        _DeleteUserUc = deleteUserUc;
        _CreateUserUc = createUserUc;
    }

    #endregion Constructors

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of all users</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting all users");
        var users = await _GetUsersUc.ExecuteAsync(new NoParams(), cancellationToken);
        return Ok(users);
    }

    /// <summary>
    /// Get a user by ID
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>User details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting user with ID: {UserId}", id);

        var user = await _GetUserUc.ExecuteAsync(id, cancellationToken);

        return Ok(user);
    }

    /// <summary>
    /// Create a new user
    /// </summary>
    /// <param name="request">User creation request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created user</returns>
    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> CreateUser([FromBody] User request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating new user with username length: {UsernameLength}", request.Username?.Length ?? 0);

        var user = await _CreateUserUc.ExecuteAsync(request, cancellationToken);

        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Update an existing user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="request">User update request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Updated user</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<User>> UpdateUser(Guid id, [FromBody] User request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating user with ID: {UserId}", id);

        var user = await _UpdateUserUc.ExecuteAsync(request, cancellationToken);

        return Ok(user);
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    /// <param name="id">User ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>No content on success</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting user with ID: {UserId}", id);

        await _DeleteUserUc.ExecuteAsync(id, cancellationToken);

        return NoContent();
    }
}
