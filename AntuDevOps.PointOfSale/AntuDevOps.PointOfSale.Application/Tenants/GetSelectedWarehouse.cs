using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Tenants;

public record GetSelectedWarehouseQuery(
    TenantId TenantId)
    : IRequest<Warehouse>;

internal class GetSelectedWarehouseQueryHandler : IRequestHandler<GetSelectedWarehouseQuery, Warehouse>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public GetSelectedWarehouseQueryHandler(ITenantRepository tenantRepository, IWarehouseRepository warehouseRepository)
    {
        _tenantRepository = tenantRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task<Warehouse> Handle(GetSelectedWarehouseQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tenant = await _tenantRepository.GetAsync(request.TenantId, cancellationToken);

        var selectedWarehousePreference = tenant.GetPreference(TenantPreferenceKeys.SelectedWarehouse);
        
        var warehouseId = int.Parse(selectedWarehousePreference.Value ?? string.Empty);

        return await _warehouseRepository.GetAsync(new WarehouseId(warehouseId), cancellationToken);
    }
}
