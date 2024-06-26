﻿using AntuDevOps.PointOfSale.Domain.Exceptions;
using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;

internal class ProductRepository : IProductRepository
{
    private readonly PointOfSaleDbContext _dbContext;

    public ProductRepository(PointOfSaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ProductId> CreateAsync(Product model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = model.ToEntity();

        await _dbContext.Products.AddAsync(entity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ProductId(entity.Id);
    }

    public async Task DeleteAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var product = await GetOrDefaultAsync(id, cancellationToken);

        if (product is null)
            return;

        _dbContext.Products.Remove(product.ToEntity());

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<Product>> FindAsync(IFindQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var pagedResult = await _dbContext.Products
            .AsNoTracking()
            .Find(query)
            .ToPagedResultAsync(query, _dbContext.Products, cancellationToken);

        return pagedResult.Map(entity => entity.ToModel());
    }

    public async Task<IReadOnlyList<Product>> FindAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var products = await _dbContext.Products
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return products
            .Select(entity => entity.ToModel())
            .ToList();
    }

    public async Task<Product> GetAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await GetOrDefaultAsync(id, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), id);
    }

    public async Task<Product?> GetByCodeOrDefaultAsync(string code, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);

        return entity?.ToModel();
    }

    public async Task<Product?> GetOrDefaultAsync(ProductId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        return entity?.ToModel();
    }

    public async Task UpdateAsync(Product model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = model.ToEntity();

        _dbContext.Products.Update(entity);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
