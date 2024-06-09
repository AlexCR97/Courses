using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.OrderSnapshots;

public record FindOrderSnapshotsQuery(
    TenantId TenantId,
    int Page = FindQuery.PageDefault,
    int Size = FindQuery.SizeDefault,
    Sort? Sort = null,
    string? Search = null)
    : FindQuery(Page, Size, Sort, Search)
    , IRequest<PagedResult<OrderSnapshot>>;

internal class FindOrderSnapshotsQueryHandler : IRequestHandler<FindOrderSnapshotsQuery, PagedResult<OrderSnapshot>>
{
    private readonly IOrderSnapshotRepository _orderSnapshotRepository;

    public FindOrderSnapshotsQueryHandler(IOrderSnapshotRepository orderSnapshotRepository)
    {
        _orderSnapshotRepository = orderSnapshotRepository;
    }

    public async Task<PagedResult<OrderSnapshot>> Handle(FindOrderSnapshotsQuery query, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // TODO Use TenantId

        return await _orderSnapshotRepository.FindAsync(query, cancellationToken);
    }
}
