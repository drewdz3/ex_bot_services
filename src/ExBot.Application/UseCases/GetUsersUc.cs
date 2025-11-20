using ExBot.Domain.Entities;
using ExBot.Domain.Repositories;

namespace ExBot.Application.UseCases
{
    public interface IGetUsersUc : IUseCase<List<User>, NoParams>
    {
    }

    public class GetUsersUc : IGetUsersUc
    {
        private readonly IUserRepository _UserRepository;

        public GetUsersUc(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
        }

        public async Task<List<User>> ExecuteAsync(NoParams param, CancellationToken cancellationToken)
        {
            return await _UserRepository.GetAllAsync(cancellationToken);
        }
    }
}
