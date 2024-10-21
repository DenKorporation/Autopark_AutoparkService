using AutoparkService.Domain.Specifications.Common;

namespace AutoparkService.Domain.Repositories.Interfaces;

public interface IRepository<TEntity>
    where TEntity : class
{
    public Task<IReadOnlyCollection<TDestination>> GetAllAsync<TDestination>(
        int pageNumber,
        int pageSize,
        Func<IQueryable<TEntity>, IQueryable<TDestination>> mapTo,
        Specification<TEntity>? specification = default,
        CancellationToken cancellationToken = default);

    public Task<int> CountAsync(
        Specification<TEntity>? specification = default,
        CancellationToken cancellationToken = default);

    public Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);
    public Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken = default);
}
