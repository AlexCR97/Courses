using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;

internal class OrderRepository : IOrderRepository
{
    private readonly PointOfSaleDbContext _dbContext;
    private readonly IServiceProvider _serviceProvider;

    public OrderRepository(PointOfSaleDbContext dbContext, IServiceProvider serviceProvider)
    {
        _dbContext = dbContext;
        _serviceProvider = serviceProvider;
    }

    public async Task<OrderId> CreateAsync(Order orderModel, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var orderEntity = orderModel.ToEntity();

        _dbContext.ChangeTracker.Clear();
        await _dbContext.Orders.AddAsync(orderEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var orderLineEntities = orderModel.Lines.Select(orderLineModel => new OrderLineEntity(
            orderEntity.Id,
            null,
            orderLineModel.Product.ProductId.Value,
            null,
            orderLineModel.Quantity));

        _dbContext.ChangeTracker.Clear();
        await _dbContext.OrderLines.AddRangeAsync(orderLineEntities, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new OrderId(orderEntity.Id);
    }

    public async Task DeleteAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var order = await GetOrDefaultAsync(id, cancellationToken);

        if (order is null)
            return;

        _dbContext.Orders.Remove(order.ToEntity());

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedResult<Order>> FindAsync(IFindQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var pagedResult = await _dbContext.Orders
            .AsNoTracking()
            .Find(query)
            .ToPagedResultAsync(query, _dbContext.Orders, cancellationToken);

        var tasks = pagedResult.Results.Select(x =>
        {
            var dbContext = _serviceProvider.GetRequiredService<PointOfSaleDbContext>();
            return ToModelAsync(dbContext, x, cancellationToken);
        });
        var orders = await Task.WhenAll(tasks);

        return pagedResult.WithResults(orders);
    }

    public async Task<Order> GetAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await GetOrDefaultAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"No such Order with Id={id}");
    }

    public async Task<Order?> GetOrDefaultAsync(OrderId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null)
            return null;

        return await ToModelAsync(_dbContext, entity, cancellationToken);
    }

    public async Task UpdateAsync(Order orderModel, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var orderEntity = orderModel.ToEntity();

        _dbContext.ChangeTracker.Clear();
        _dbContext.Orders.Update(orderEntity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        foreach (var orderLineModel in orderModel.Lines)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var existingOrderLine = await _dbContext.OrderLines
                .AsNoTracking()
                .Where(x => true
                    && x.OrderId == orderEntity.Id
                    && x.ProductId == orderLineModel.Product.ProductId.Value)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingOrderLine is not null)
            {
                existingOrderLine.Update(orderLineModel.Quantity);

                _dbContext.ChangeTracker.Clear();
                _dbContext.OrderLines.Update(existingOrderLine);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                var newOrderLine = OrderLineEntity.Create(
                    orderEntity.Id,
                    orderLineModel.Product.ProductId.Value,
                    orderLineModel.Quantity);

                _dbContext.ChangeTracker.Clear();
                await _dbContext.OrderLines.AddAsync(newOrderLine, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }

    private static async Task<Order> ToModelAsync(PointOfSaleDbContext dbContext, OrderEntity a, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var orderLineEntities = await dbContext.OrderLines
            .AsNoTracking()
            .Where(x => x.OrderId == a.Id)
            .Include(x => x.Product)
            .ToListAsync();

        var orderLines = orderLineEntities.Select(x => new OrderLine(
            new OrderLineProduct(
                new ProductId(x.Product!.Id),
                x.Product!.Code,
                x.Product!.DisplayName),
            x.Quantity));

        return a.ToModel(orderLines);
    }
}
