using AutoparkService.Domain.Repositories.Interfaces;
using AutoparkService.Domain.Specifications.Common;
using AutoparkService.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace AutoparkService.Infrastructure.Repositories.Implementations;

public class Repository<TEntity>(AutoParkDbContext dbContext)
    : IRepository<TEntity>
    where TEntity : class
{
    public async Task<IReadOnlyCollection<TDestination>> GetAllAsync<TDestination>(
        int pageNumber,
        int pageSize,
        Func<IQueryable<TEntity>, IQueryable<TDestination>> mapTo,
        Specification<TEntity>? specification = default,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = dbContext
            .Set<TEntity>()
            .AsNoTracking()
            .OrderBy(e => EF.Property<Guid>(e, "Id"));

        if (specification is not null)
        {
            query = query.Where(specification.Criteria);
        }

        var destQuery = mapTo(query);

        return await destQuery
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        Specification<TEntity>? specification = default,
        CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<TEntity>().AsNoTracking();

        if (specification is not null)
        {
            query = query.Where(specification.Criteria);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext
            .Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);
    }

    public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await dbContext
            .Set<TEntity>()
            .AddAsync(entity, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext
            .Set<TEntity>()
            .Update(entity);

        await dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        dbContext
            .Set<TEntity>()
            .Remove(entity);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<bool> ExistAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return dbContext
            .Set<TEntity>()
            .AnyAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);
    }
}
