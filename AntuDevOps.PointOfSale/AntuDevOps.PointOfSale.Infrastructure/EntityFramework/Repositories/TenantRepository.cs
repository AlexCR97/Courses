using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;

internal class TenantRepository : ITenantRepository
{
    private readonly PointOfSaleDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    public TenantRepository(PointOfSaleDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
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

    public async Task<PagedResult<Tenant>> FindAsync(IFindQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tenants = await _dbContext.Tenants
            .AsNoTracking()
            .Find(query)
            .ToPagedResultAsync(query, _dbContext.Tenants, cancellationToken);

        var tenantEntityTasks = tenants.Results.Select(tenant =>
        {
            var dbContext = _serviceProvider.GetRequiredService<PointOfSaleDbContext>();
            return ToModelAsync(dbContext, tenant, cancellationToken);
        });

        var tenantEntities = await Task.WhenAll(tenantEntityTasks);

        return tenants.WithResults(tenantEntities);
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

        if (entity is null)
            return null;

        return await ToModelAsync(_dbContext, entity, cancellationToken);
    }

    public async Task<Tenant?> GetOrDefaultAsync(TenantId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.Tenants
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null)
            return null;

        return await ToModelAsync(_dbContext, entity, cancellationToken);
    }

    public async Task UpdateAsync(Tenant tenantModel, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tenantEntity = tenantModel.ToEntity();

        _dbContext.ChangeTracker.Clear();
        _dbContext.Tenants.Update(tenantEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var preferenceModel in tenantModel.Preferences)
        {
            var existingPreferenceEntity = await _dbContext.TenantPreferences
                .AsNoTracking()
                .Where(x => x.TenantId == tenantEntity.Id && x.Key == preferenceModel.Key)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingPreferenceEntity is not null)
            {
                existingPreferenceEntity.SetValue(preferenceModel.Value);
                _dbContext.ChangeTracker.Clear();
                _dbContext.TenantPreferences.Update(existingPreferenceEntity);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                var newTenantPreferenceEntity = new TenantPreferenceEntity(
                    tenantEntity.Id,
                    null,
                    preferenceModel.Key,
                    preferenceModel.Value);

                _dbContext.ChangeTracker.Clear();
                await _dbContext.TenantPreferences.AddAsync(newTenantPreferenceEntity, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }

    private static async Task<Tenant> ToModelAsync(PointOfSaleDbContext dbContext, TenantEntity tenantEntity, CancellationToken cancellationToken)
    {
        var tenantPreferenceEntities = await dbContext.TenantPreferences
            .AsNoTracking()
            .Where(x => x.TenantId == tenantEntity.Id)
            .ToListAsync(cancellationToken);

        var tenantPreferences = tenantPreferenceEntities
            .Select(x => x.ToModel())
            .ToList();

        return tenantEntity.ToModel(tenantPreferences);
    }
}
