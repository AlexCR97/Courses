using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Warehouses;

public record FindWarehousesQuery(
    TenantId TenantId,
    int Page = FindQuery.PageDefault,
    int Size = FindQuery.SizeDefault,
    Sort? Sort = null,
    string? Search = null)
    : FindQuery(Page, Size, Sort, Search)
    , IRequest<PagedResult<Warehouse>>;

internal class FindWarehousesQueryHandler : IRequestHandler<FindWarehousesQuery, PagedResult<Warehouse>>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public FindWarehousesQueryHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<PagedResult<Warehouse>> Handle(FindWarehousesQuery query, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        // TODO Use TenantId

        return await _warehouseRepository.FindAsync(query, cancellationToken);
    }
}
