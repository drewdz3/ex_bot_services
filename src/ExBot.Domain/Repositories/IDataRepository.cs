namespace ExBot.Domain.Repositories
{
    public interface IDataRepository<TModel, TKey> where TModel : class
    {
        /// <summary>
        /// Retrieve an entity by its unique identifier.
        /// </summary>
        /// <param name="key">The unique identifier of the record to retrieve</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An instance of TModel or default</returns>
        Task<TModel?> GetByIdAsync(TKey key, CancellationToken cancellationToken);

        /// <summary>
        /// Retreive all available entities.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An enumerable of TModel</returns>
        Task<List<TModel>> GetAllAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Add a new entity to the data store.
        /// </summary>
        /// <param name="model">The instance of the entity to add.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<TModel> AddAsync(TModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Update an existing entity in the data store.
        /// </summary>
        /// <param name="model">The entity to update</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<TModel> UpdateAsync(TModel model, CancellationToken cancellationToken);

        /// <summary>
        /// Delete an entity from the data store by its unique identifier.
        /// </summary>
        /// <param name="key">The unique identifier of the entity</param>
        /// <param name="cancellationToken">cancellation token</param>
        Task DeleteAsync(TKey key, CancellationToken cancellationToken);
    }
}
