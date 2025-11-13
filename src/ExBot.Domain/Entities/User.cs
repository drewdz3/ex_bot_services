using ExBot.Domain.Common;

namespace ExBot.Domain.Entities;

/// <summary>
/// User entity representing a mobile app user
/// </summary>
public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}
