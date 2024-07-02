using AntuDevOps.PointOfSale.Application.Warehouses;
using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Orders;

public record IncreaseOrderProductQuantityCommand(
    OrderId OrderId,
    WarehouseId WarehouseId,
    ProductId ProductId,
    string? LastModifiedBy)
    : IRequest;

internal class IncreaseOrderProductQuantityRequestHandler : IRequestHandler<IncreaseOrderProductQuantityCommand>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISender _sender;

    public IncreaseOrderProductQuantityRequestHandler(IOrderRepository orderRepository, ISender sender)
    {
        _orderRepository = orderRepository;
        _sender = sender;
    }

    public async Task Handle(IncreaseOrderProductQuantityCommand command, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var order = await _orderRepository.GetAsync(command.OrderId, cancellationToken);

        var stock = await _sender.Send(new GetStockQuery(command.WarehouseId, command.ProductId), cancellationToken);

        if (stock is null)
            throw new Exception("No such product in warehouse");

        if (stock.OutOfStock)
            throw new Exception("Out of stock.");

        var orderLine = order.GetLine(stock.Product.Id);

        if (!stock.HasCapacity(orderLine.Quantity + 1))
            throw new Exception("No capacity available.");

        order.IncreaseProductQuantity(stock.Product, command.LastModifiedBy);

        await _orderRepository.UpdateAsync(order, cancellationToken);
    }
}
