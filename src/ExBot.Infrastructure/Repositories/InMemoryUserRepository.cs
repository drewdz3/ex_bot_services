using ExBot.Domain.Entities;
using ExBot.Domain.Repositories;

namespace ExBot.Infrastructure.Repositories;

/// <summary>
/// In-memory implementation of User repository for demonstration
/// In production, this would be replaced with actual database implementation
/// </summary>
public class InMemoryUserRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IEnumerable<User>>(_users);
    }

    public Task<User> CreateAsync(User user, CancellationToken cancellationToken = default)
    {
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task<User> UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            _users.Remove(existingUser);
            _users.Add(user);
        }
        return Task.FromResult(user);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
            return Task.FromResult(true);
        }
        return Task.FromResult(false);
    }
}
