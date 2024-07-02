using AntuDevOps.PointOfSale.Application.Warehouses;
using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Orders;

public record CreateOrderCommand(
    TenantId TenantId,
    WarehouseId WarehouseId,
    ProductId ProductId,
    string CreatedBy)
    : IRequest<OrderId>;

internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, OrderId>
{
    private readonly IOrderRepository _orderRepository;
    private readonly ISender _sender;
    private readonly ITenantRepository _tenantRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository, ISender sender, ITenantRepository tenantRepository)
    {
        _orderRepository = orderRepository;
        _sender = sender;
        _tenantRepository = tenantRepository;
    }

    public async Task<OrderId> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tenant = await _tenantRepository.GetAsync(command.TenantId, cancellationToken);

        var stock = await _sender.Send(new GetStockQuery(command.WarehouseId, command.ProductId), cancellationToken);

        if (stock is null)
            throw new Exception("No such product in warehouse.");

        if (stock.OutOfStock)
            throw new Exception("Out of stock.");

        // TODO Fix with error handling
        //if (!stock.HasCapacity(1))
        //    throw new Exception("No capacity available.");

        var order = Order.Create(tenant, stock.Product, command.CreatedBy);

        return await _orderRepository.CreateAsync(order, cancellationToken);
    }
}
