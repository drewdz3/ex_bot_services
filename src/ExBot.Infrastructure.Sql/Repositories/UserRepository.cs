using ExBot.Domain.Entities;
using ExBot.Domain.Repositories;
using ExBot.Infrastructure.Sql.Dto;
using Microsoft.EntityFrameworkCore;
using ObjectMapper;

namespace ExBot.Infrastructure.Sql.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region Fields

        private readonly SqlDbContext _DbContext;
        private readonly IConversionSet<User, UserDto> _UserToDbMapping;
        private readonly IConversionSet<UserDto, User> _UserFromDbMapping;

        #endregion Fields

        #region Constructors

        public UserRepository(SqlDbContext dbContext, IConversionSet<User, UserDto> userToDbMapping, IConversionSet<UserDto, User> userFromDbMapping)
        {
            _DbContext = dbContext;
            _UserToDbMapping = userToDbMapping;
            _UserFromDbMapping = userFromDbMapping;
        }

        #endregion Constructors

        #region Implementation

        public async Task<User> AddAsync(User model, CancellationToken cancellationToken)
        {
            if (model == null) throw new ArgumentNullException("Unable to add null user");
            //  first convert to dto
            var dto = _UserToDbMapping.Map(model);
            await _DbContext.AddAsync(dto);
            return model;
        }

        public async Task DeleteAsync(Guid key, CancellationToken cancellationToken)
        {
            var user = await _DbContext.FindAsync<UserDto>(key);
            if (user == null) return;
            _DbContext.Remove(user);
        }

        public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
        {
            //  get users from the context
            var dbUsers = await _DbContext.Set<UserDto>().ToListAsync();
            //  map to domain entities
            var users = new List<User>();
            foreach (var dbUser in dbUsers)
            {
                users.Add(_UserFromDbMapping.Map(dbUser));
            }
            return users;
        }

        public async Task<User?> GetByIdAsync(Guid key, CancellationToken cancellationToken)
        {
            //  get the db record
            var dbUser = await _DbContext.FindAsync<UserDto>(key);
            if (dbUser == null) return null;
            return _UserFromDbMapping.Map(dbUser);
        }

        public async Task<User> UpdateAsync(User model, CancellationToken cancellationToken)
        {
            //  find the db record
            var dbUser = await _DbContext.FindAsync<UserDto>(model.Id);
            if (dbUser == null) throw new NullReferenceException("Could not find the user record to update.");
            //  map the changes
            dbUser = _UserToDbMapping.Map(model);
            _DbContext.Update(dbUser);
            return model;
        }

        #endregion Implementation
    }
}
