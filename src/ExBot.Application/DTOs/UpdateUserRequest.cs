namespace ExBot.Application.DTOs;

/// <summary>
/// Request DTO for updating a user
/// </summary>
public class UpdateUserRequest
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? DisplayName { get; set; }
    public bool? IsActive { get; set; }
}
