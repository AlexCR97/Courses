using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Orders;

public record GetOrderQuery(
    OrderId OrderId)
    : IRequest<Order>;

internal class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Order>
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _orderRepository.GetAsync(request.OrderId, cancellationToken);
    }
}
