using AntuDevOps.PointOfSale.Application.Warehouses;
using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Orders;

public record DecreaseOrderProductQuantityCommand(
    OrderId OrderId,
    WarehouseId WarehouseId,
    ProductId ProductId,
    string? LastModifiedBy)
    : IRequest<OrderProductQuantityDecreasedResult>;

public record OrderProductQuantityDecreasedResult(
    bool ProductRemoved);

internal class DecreaseOrderProductQuantityCommandHandler : IRequestHandler<DecreaseOrderProductQuantityCommand, OrderProductQuantityDecreasedResult>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISender _sender;

    public DecreaseOrderProductQuantityCommandHandler(IOrderRepository orderRepository, ISender sender)
    {
        _orderRepository = orderRepository;
        _sender = sender;
    }

    public async Task<OrderProductQuantityDecreasedResult> Handle(DecreaseOrderProductQuantityCommand command, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var order = await _orderRepository.GetAsync(command.OrderId, cancellationToken);

        var stock = await _sender.Send(new GetStockQuery(command.WarehouseId, command.ProductId), cancellationToken);

        if (stock is null)
            throw new Exception("No such product in warehouse");

        var productRemoved = order.DecreaseProductQuantity(stock.Product, command.LastModifiedBy);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new OrderProductQuantityDecreasedResult(productRemoved);
    }
}
