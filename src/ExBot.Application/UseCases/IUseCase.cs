namespace ExBot.Application.UseCases
{
    public interface IUseCase<TResult, TParam>
    {
        Task<TResult> ExecuteAsync(TParam param, CancellationToken cancellationToken);
    }

    public class NoParams { }
}

