using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Orders;

public record FindOrdersQuery(
    TenantId TenantId,
    int Page = FindQuery.PageDefault,
    int Size = FindQuery.SizeDefault,
    Sort? Sort = null,
    string? Search = null)
    : FindQuery(Page, Size, Sort, Search)
    , IRequest<PagedResult<Order>>;

internal class FindOrdersQueryHandler : IRequestHandler<FindOrdersQuery, PagedResult<Order>>
{
    private readonly IOrderRepository _orderRepository;

    public FindOrdersQueryHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<PagedResult<Order>> Handle(FindOrdersQuery query, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // TODO Use TenantId

        return await _orderRepository.FindAsync(query, cancellationToken);
    }
}
