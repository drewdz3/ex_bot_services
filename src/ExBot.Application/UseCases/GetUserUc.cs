using ExBot.Domain.Entities;
using ExBot.Domain.Repositories;

namespace ExBot.Application.UseCases
{
    public interface IGetUserUc : IUseCase<User?, Guid>
    {
    }

    public class GetUserUc : IGetUserUc
    {
        #region Fields

        private readonly IUserRepository _UserRepository;

        #endregion Fields

        #region Constructors

        public GetUserUc(IUserRepository userRepository)
        {
            _UserRepository = userRepository;
        }

        #endregion Constructors

        #region Implementation

        public async Task<User?> ExecuteAsync(Guid param, CancellationToken cancellationToken)
        {
            return await _UserRepository.GetByIdAsync(param, cancellationToken);
        }

        #endregion Implementation
    }
}
