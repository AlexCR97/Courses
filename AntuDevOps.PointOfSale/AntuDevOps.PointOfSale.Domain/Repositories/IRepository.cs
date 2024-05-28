namespace AntuDevOps.PointOfSale.Domain.Repositories;

public interface IRepository<TModel, TId> where TModel : class
{
    Task<TId> CreateAsync(TModel model, CancellationToken cancellationToken = default);
    Task DeleteAsync(TId id, CancellationToken cancellationToken = default);
    Task<PagedResult<TModel>> FindAsync(IFindQuery query, CancellationToken cancellationToken = default);
    Task<TModel> GetAsync(TId id, CancellationToken cancellationToken = default);
    Task<TModel?> GetOrDefaultAsync(TId id, CancellationToken cancellationToken = default);
    Task UpdateAsync(TModel model, CancellationToken cancellationToken = default);
}
