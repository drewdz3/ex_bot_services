using System.ComponentModel.DataAnnotations.Schema;

namespace ExBot.Infrastructure.Sql.Dto
{
    [Table("Users")]
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
