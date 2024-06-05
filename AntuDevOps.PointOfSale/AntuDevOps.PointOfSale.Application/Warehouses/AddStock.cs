using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Warehouses;

public record AddStockCommand(
    WarehouseId WarehouseId,
    ProductId ProductId,
    int Quantity,
    decimal Price)
    : IRequest;

internal class AddStockCommandHandler : IRequestHandler<AddStockCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public AddStockCommandHandler(IProductRepository productRepository, IWarehouseRepository warehouseRepository)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task Handle(AddStockCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var warehouse = await _warehouseRepository.GetAsync(request.WarehouseId, cancellationToken);

        var product = await _productRepository.GetAsync(request.ProductId, cancellationToken);

        warehouse.AddStock(product, request.Quantity, request.Price);

        await _warehouseRepository.UpdateAsync(warehouse, cancellationToken);
    }
}
