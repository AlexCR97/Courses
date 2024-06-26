﻿using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;

internal class UserRepository : IUserRepository
{
    private readonly PointOfSaleDbContext _dbContext;

    public UserRepository(PointOfSaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UserId> CreateAsync(User userModel, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var userEntity = userModel.ToEntity();
        
        await _dbContext.Users.AddAsync(userEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var tenantUserEntities = userModel.Tenants.Select(userTenant => new TenantUserEntity(
            userTenant.TenantId.Value,
            null,
            userEntity.Id,
            null,
            userTenant.Role.ToString()));
        
        await _dbContext.TenantUsers.AddRangeAsync(tenantUserEntities);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UserId(userEntity.Id);
    }

    public async Task DeleteAsync(UserId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await GetOrDefaultAsync(id, cancellationToken);

        if (user is null)
            return;

        _dbContext.Users.Remove(user.ToEntity());

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<User>> FindAsync(IFindQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var pagedUserEntities = await _dbContext.Users
            .AsNoTracking()
            .Find(query)
            .ToPagedResultAsync(query, _dbContext.Users, cancellationToken);

        var mapTasks = pagedUserEntities.Results.Select(userEntity => ToModelAsync(userEntity, cancellationToken));
        var mapTaskResults = await Task.WhenAll(mapTasks);

        return pagedUserEntities.WithResults(mapTaskResults);
    }

    private async Task<User> ToModelAsync(UserEntity userEntity, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tenantUserEntities = await _dbContext.TenantUsers
            .AsNoTracking()
            .Where(x => x.UserId == userEntity.Id)
            .ToListAsync(cancellationToken);

        return userEntity.ToModel(tenantUserEntities);
    }

    public async Task<User> GetAsync(UserId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await GetOrDefaultAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"No such User with Id={id}");
    }

    public async Task<User?> GetByEmailOrDefaultAsync(Email email, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.EmailNormalized == email.NormalizedValue, cancellationToken);

        if (entity is null)
            return null;

        return await ToModelAsync(entity, cancellationToken);
    }

    public async Task<User?> GetOrDefaultAsync(UserId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null)
            return null;

        return await ToModelAsync(entity, cancellationToken);
    }

    public async Task UpdateAsync(User model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = model.ToEntity();

        _dbContext.Users.Update(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
