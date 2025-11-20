using ExBot.Domain.Entities;
using ExBot.Domain.Repositories;

namespace ExBot.Application.UseCases
{
    public interface ICreateUserUc : IUseCase<User, User>
    {
    }

    internal class CreateUserUc : ICreateUserUc
    {
        private readonly IUserRepository _Repository;

        public CreateUserUc(IUserRepository userRepository)
        {            
            _Repository = userRepository;
        }

        public Task<User> ExecuteAsync(User param, CancellationToken cancellationToken)
        {
            //  TODO: validate user object
            //  create user
            return _Repository.AddAsync(param, cancellationToken);
        }
    }
}
