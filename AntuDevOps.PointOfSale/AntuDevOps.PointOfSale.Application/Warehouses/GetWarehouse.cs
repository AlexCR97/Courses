using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Warehouses;

public record GetWarehouseQuery(
    WarehouseId WarehouseId)
    : IRequest<Warehouse>;

internal class GetWarehouseQueryHandler : IRequestHandler<GetWarehouseQuery, Warehouse>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public GetWarehouseQueryHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task<Warehouse> Handle(GetWarehouseQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _warehouseRepository.GetAsync(request.WarehouseId, cancellationToken);
    }
}
