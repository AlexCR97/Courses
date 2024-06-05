using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Warehouses;

public record AddStockQuantityCommand(
    WarehouseId WarehouseId,
    ProductId ProductId,
    int Quantity)
    : IRequest;

internal class AddStockQuantityCommandHandler : IRequestHandler<AddStockQuantityCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public AddStockQuantityCommandHandler(IProductRepository productRepository, IWarehouseRepository warehouseRepository)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task Handle(AddStockQuantityCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var warehouse = await _warehouseRepository.GetAsync(request.WarehouseId, cancellationToken);

        var product = await _productRepository.GetAsync(request.ProductId, cancellationToken);

        warehouse.AddStockQuantity(product, request.Quantity);

        await _warehouseRepository.UpdateAsync(warehouse, cancellationToken);
    }
}
