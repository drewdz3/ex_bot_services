namespace ExBot.Application.DTOs;

/// <summary>
/// Request DTO for creating a new user
/// </summary>
public class CreateUserRequest
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}
