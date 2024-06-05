using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Warehouses;

public record CreateWarehouseCommand(
    TenantId TenantId,
    string Code,
    string DisplayName,
    string CreatedBy)
    : IRequest<WarehouseId>;

internal class CreateWarehouseCommandHandler : IRequestHandler<CreateWarehouseCommand, WarehouseId>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public CreateWarehouseCommandHandler(ITenantRepository tenantRepository, IWarehouseRepository warehouseRepository)
    {
        _tenantRepository = tenantRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task<WarehouseId> Handle(CreateWarehouseCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tenant = await _tenantRepository.GetAsync(request.TenantId, cancellationToken);

        var warehouse = Warehouse.Create(
            tenant,
            request.CreatedBy,
            request.Code,
            request.DisplayName);

        return await _warehouseRepository.CreateAsync(warehouse, cancellationToken);
    }
}
