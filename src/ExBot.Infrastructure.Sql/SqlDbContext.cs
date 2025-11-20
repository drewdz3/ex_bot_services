using ExBot.Infrastructure.Sql.Dto;
using Microsoft.EntityFrameworkCore;

namespace ExBot.Infrastructure.Sql;

/// <summary>
/// Entity Framework Core database context for relational data
/// </summary>
public class SqlDbContext : DbContext
{
    public SqlDbContext(DbContextOptions<SqlDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserDto>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.DisplayName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });
    }
}
