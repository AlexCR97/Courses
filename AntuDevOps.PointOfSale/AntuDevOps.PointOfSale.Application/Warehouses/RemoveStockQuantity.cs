using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Warehouses;

public record RemoveStockQuantityCommand(
    WarehouseId WarehouseId,
    ProductId ProductId,
    int Quantity)
    : IRequest<RemoveStockQuantityResult>;

public record RemoveStockQuantityResult(
    bool OutOfStock);

internal class RemoveStockQuantityCommandHandler : IRequestHandler<RemoveStockQuantityCommand, RemoveStockQuantityResult>
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public RemoveStockQuantityCommandHandler(IProductRepository productRepository, IWarehouseRepository warehouseRepository)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task<RemoveStockQuantityResult> Handle(RemoveStockQuantityCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var warehouse = await _warehouseRepository.GetAsync(request.WarehouseId, cancellationToken);

        var product = await _productRepository.GetAsync(request.ProductId, cancellationToken);

        var outOfStock = warehouse.RemoveStockQuantity(product, request.Quantity);

        await _warehouseRepository.UpdateAsync(warehouse, cancellationToken);

        return new RemoveStockQuantityResult(outOfStock);
    }
}
