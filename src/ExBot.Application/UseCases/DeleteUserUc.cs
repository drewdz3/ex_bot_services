using ExBot.Domain.Repositories;

namespace ExBot.Application.UseCases
{
    public interface IDeleteUserUc : IUseCase<Guid, Guid>
    {
    }

    public class DeleteUserUc : IDeleteUserUc
    {
        private readonly IUserRepository _Repository;

        public DeleteUserUc(IUserRepository repository)
        {
            _Repository = repository;
        }

        public async Task<Guid> ExecuteAsync(Guid param, CancellationToken cancellationToken)
        {
            await _Repository.DeleteAsync(param, cancellationToken);
            return param;
        }
    }
}
