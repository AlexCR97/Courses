using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Warehouses;

public record DeleteWarehouseCommand(
    WarehouseId WarehouseId)
    : IRequest;

internal class DeleteWarehouseCommandHandler : IRequestHandler<DeleteWarehouseCommand>
{
    private readonly IWarehouseRepository _warehouseRepository;

    public DeleteWarehouseCommandHandler(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public async Task Handle(DeleteWarehouseCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await _warehouseRepository.DeleteAsync(request.WarehouseId, cancellationToken);
    }
}
