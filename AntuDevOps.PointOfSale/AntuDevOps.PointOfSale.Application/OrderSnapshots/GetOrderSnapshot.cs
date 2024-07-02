using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.OrderSnapshots;

public record GetOrderSnapshotQuery(
    OrderSnapshotId OrderSnapshotId)
    : IRequest<OrderSnapshot>;

internal class GetOrderSnapshotQueryHandler : IRequestHandler<GetOrderSnapshotQuery, OrderSnapshot>
{
    private readonly IOrderSnapshotRepository _orderRepository;

    public GetOrderSnapshotQueryHandler(IOrderSnapshotRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderSnapshot> Handle(GetOrderSnapshotQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _orderRepository.GetAsync(request.OrderSnapshotId, cancellationToken);
    }
}
