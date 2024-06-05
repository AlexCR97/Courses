using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Warehouses;

public record RemoveStockCommand(
    WarehouseId WarehouseId,
    ProductId ProductId)
    : IRequest;

internal class RemoveStockCommandHandler : IRequestHandler<RemoveStockCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public RemoveStockCommandHandler(IProductRepository productRepository, IWarehouseRepository warehouseRepository)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task Handle(RemoveStockCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var warehouse = await _warehouseRepository.GetAsync(request.WarehouseId, cancellationToken);

        var product = await _productRepository.GetAsync(request.ProductId, cancellationToken);

        warehouse.RemoveStock(product);

        await _warehouseRepository.UpdateAsync(warehouse, cancellationToken);
    }
}
