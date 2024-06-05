using AntuDevOps.PointOfSale.Application.Tenants;
using AntuDevOps.PointOfSale.Application.Warehouses;
using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Orders;

public record SetOrderStatusRequest(
    TenantId TenantId,
    OrderId OrderId,
    OrderStatus Status,
    string? LastModifiedBy)
    : IRequest;

internal class SetOrderStatusRequestHandler : IRequestHandler<SetOrderStatusRequest>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderSnapshotRepository _orderSnapshotRepository;
    private readonly ISender _sender;
    private readonly ITenantRepository _tenantRepository;

    public SetOrderStatusRequestHandler(IOrderRepository orderRepository, IOrderSnapshotRepository orderSnapshotRepository, ISender sender, ITenantRepository tenantRepository)
    {
        _orderRepository = orderRepository;
        _orderSnapshotRepository = orderSnapshotRepository;
        _sender = sender;
        _tenantRepository = tenantRepository;
    }

    public async Task Handle(SetOrderStatusRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tenant = await _tenantRepository.GetAsync(request.TenantId, cancellationToken);

        var order = await _orderRepository.GetAsync(request.OrderId, cancellationToken);

        order.SetStatus(request.Status, request.LastModifiedBy);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        await ProcessStatusAsync(
            tenant,
            order,
            request.LastModifiedBy
                ?? order.LastModifiedBy
                ?? order.CreatedBy,
            cancellationToken);
    }

    private Task ProcessStatusAsync(Tenant tenant, Order order, string lastModifiedBy, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (order.Status == OrderStatus.Drafted)
        {
            return Task.CompletedTask;
        }

        if (order.Status == OrderStatus.Processing)
        {
            return Task.CompletedTask;
        }

        if (order.Status == OrderStatus.Cancelled)
        {
            return Task.CompletedTask;
        }

        if (order.Status == OrderStatus.Completed)
        {
            return ProcessCompletedStatusAsync(tenant, order, lastModifiedBy, cancellationToken);
        }

        return Task.CompletedTask;
    }

    private async Task ProcessCompletedStatusAsync(Tenant tenant, Order order, string lastModifiedBy, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var selectedWarehouse = await _sender.Send(new GetSelectedWarehouseQuery(tenant.Id), cancellationToken);

        var orderLineSnapshotTasks = order.Lines.Select(line => TakeOrderLineSnapshotAsync(
            line,
            selectedWarehouse.Id,
            cancellationToken));

        var orderLineSnapshots = await Task.WhenAll(orderLineSnapshotTasks);

        var orderSnapshot = OrderSnapshot.Create(
            tenant.Id,
            order.Id,
            order.Status,
            lastModifiedBy,
            orderLineSnapshots);

        await _orderSnapshotRepository.CreateAsync(orderSnapshot, cancellationToken);

        var removeStockQuantityTasks = order.Lines.Select(line => _sender.Send(new RemoveStockQuantityCommand(
            selectedWarehouse.Id,
            line.Product.ProductId,
            line.Quantity)));

        await Task.WhenAll(removeStockQuantityTasks);
    }

    private async Task<OrderLineSnapshot> TakeOrderLineSnapshotAsync(OrderLine line, WarehouseId warehouseId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var stock = await _sender.Send(new GetStockQuery(
            warehouseId,
            line.Product.ProductId),
            cancellationToken);

        if (stock is null)
            throw new Exception("No such product in warehouse.");

        if (stock.OutOfStock)
            throw new Exception("Out of stock.");

        if (!stock.HasCapacity(line.Quantity))
            throw new Exception("No capacity available.");

        return new OrderLineSnapshot(
            new ProductSnapshot(
                stock.Product.Id,
                stock.Product.CreatedAt,
                stock.Product.CreatedBy,
                stock.Product.Code,
                stock.Product.DisplayName,
                stock.Price),
            line.Quantity);
    }
}
