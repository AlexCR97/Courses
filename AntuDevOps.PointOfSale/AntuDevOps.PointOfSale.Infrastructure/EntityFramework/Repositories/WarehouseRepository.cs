using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;

internal class WarehouseRepository : IWarehouseRepository
{
    private readonly PointOfSaleDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    public WarehouseRepository(PointOfSaleDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    public async Task<WarehouseId> CreateAsync(Warehouse model, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = model.ToEntity();

        await _dbContext.Warehouses.AddAsync(entity, cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new WarehouseId(entity.Id);
    }

    public async Task DeleteAsync(WarehouseId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var warehouse = await GetOrDefaultAsync(id, cancellationToken);

        if (warehouse is null)
            return;

        _dbContext.Warehouses.Remove(warehouse.ToEntity());

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<Warehouse>> FindAsync(IFindQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var pagedResult = await _dbContext.Warehouses
            .AsNoTracking()
            .Find(query)
            .ToPagedResultAsync(query, _dbContext.Warehouses, cancellationToken);

        var tasks = pagedResult.Results.Select(entity =>
        {
            var dbContext = _serviceProvider.GetRequiredService<PointOfSaleDbContext>();
            return ToModelAsync(dbContext, entity, cancellationToken);
        });

        var warehouses = await Task.WhenAll(tasks);

        return pagedResult.WithResults(warehouses);
    }

    public async Task<Warehouse> GetAsync(WarehouseId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await GetOrDefaultAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"No such Warehouse with Id={id}");
    }

    public async Task<Warehouse?> GetByCodeOrDefaultAsync(string code, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Code == code, cancellationToken);

        if (entity is null)
            return null;

        return await ToModelAsync(_dbContext, entity, cancellationToken);
    }

    public async Task<Warehouse?> GetOrDefaultAsync(WarehouseId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.Warehouses
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null)
            return null;

        return await ToModelAsync(_dbContext, entity, cancellationToken);
    }

    public async Task UpdateAsync(Warehouse warehouseModel, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var warehouseEntity = warehouseModel.ToEntity();

        _dbContext.ChangeTracker.Clear();
        _dbContext.Warehouses.Update(warehouseEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var stockModel in warehouseModel.Stock)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var existingStock = await _dbContext.WarehouseStock
                .AsNoTracking()
                .Where(x => true
                    && x.WarehouseId == warehouseEntity.Id
                    && x.ProductId == stockModel.Product.ProductId.Value)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingStock is not null)
            {
                existingStock.Update(stockModel.Quantity, stockModel.Price);

                _dbContext.ChangeTracker.Clear();
                _dbContext.WarehouseStock.Update(existingStock);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
            else
            {
                var newStock = WarehouseStockEntity.Create(
                    warehouseEntity.Id,
                    stockModel.Product.ProductId.Value,
                    stockModel.Quantity,
                    stockModel.Price);

                _dbContext.ChangeTracker.Clear();
                await _dbContext.WarehouseStock.AddAsync(newStock, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }

    private static async Task<Warehouse> ToModelAsync(PointOfSaleDbContext dbContext, WarehouseEntity warehouseEntity, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var warehouseStockEntities = await dbContext.WarehouseStock
            .AsNoTracking()
            .Where(x => x.WarehouseId == warehouseEntity.Id)
            .Include(x => x.Product)
            .ToListAsync(cancellationToken);

        var stockModels = warehouseStockEntities
            .Select(b => new Stock(
                new StockProduct(
                    new ProductId(b.Product!.Id),
                    b.Product!.Code,
                    b.Product!.DisplayName),
                b.Quantity,
                b.Price))
            .ToList();

        return warehouseEntity.ToModel(stockModels);
    }
}
