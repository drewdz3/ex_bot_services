using ExBot.Domain.Entities;
using ExBot.Domain.Repositories;

namespace ExBot.Application.UseCases
{
    public interface IUpdateUserUc : IUseCase<User, User>
    {
    }

    public class UpdateUserUc : IUpdateUserUc
    {
        private readonly IUserRepository _Repository;

        public UpdateUserUc(IUserRepository repository)
        {
            _Repository = repository;
        }

        public Task<User> ExecuteAsync(User param, CancellationToken cancellationToken)
        {
            //  TODO: validate user data 
            //  update the user
            return _Repository.UpdateAsync(param, cancellationToken);
        }
    }
}
