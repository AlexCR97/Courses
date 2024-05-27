using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;

internal class TenantRepository : ITenantRepository
{
    private readonly PointOfSaleDbContext _dbContext;

    public TenantRepository(PointOfSaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TenantId> CreateAsync(Tenant model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = model.ToEntity();

        await _dbContext.Tenants.AddAsync(entity, cancellationToken);
        
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new TenantId(entity.Id);
    }

    public async Task DeleteAsync(TenantId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tenant = await GetOrDefaultAsync(id, cancellationToken);

        if (tenant is null)
            return;

        _dbContext.Tenants.Remove(tenant.ToEntity());

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Tenant>> FindAsync(IFindQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entities = await _dbContext.Tenants
            .AsQueryable()
            .Find(query)
            .ToListAsync(cancellationToken);

        return entities
            .Select(entity => entity.ToModel())
            .ToList();
    }

    public async Task<Tenant> GetAsync(TenantId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await GetOrDefaultAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"No such Tenant with Id={id}");
    }

    public async Task<Tenant?> GetByNameOrDefaultAsync(string name, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Name == name, cancellationToken);

        return entity?.ToModel();
    }

    public async Task<Tenant?> GetOrDefaultAsync(TenantId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity?.ToModel();
    }

    public async Task UpdateAsync(Tenant model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = model.ToEntity();

        _dbContext.Tenants.Update(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
