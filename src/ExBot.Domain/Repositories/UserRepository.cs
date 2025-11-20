using ExBot.Domain.Entities;

namespace ExBot.Domain.Repositories
{
    public interface IUserRepository : IDataRepository<User, Guid>
    {
    }
}