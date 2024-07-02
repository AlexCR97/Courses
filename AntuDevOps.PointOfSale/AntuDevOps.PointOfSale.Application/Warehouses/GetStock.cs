using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Warehouses;

public record GetStockQuery(
    WarehouseId WarehouseId,
    ProductId ProductId)
    : IRequest<StockSnapshot?>;

internal class GetStockQueryHandler : IRequestHandler<GetStockQuery, StockSnapshot?>
{
    private readonly IProductRepository _productRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    public GetStockQueryHandler(IProductRepository productRepository, IWarehouseRepository warehouseRepository)
    {
        _productRepository = productRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task<StockSnapshot?> Handle(GetStockQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var warehouse = await _warehouseRepository.GetAsync(request.WarehouseId, cancellationToken);

        var product = await _productRepository.GetAsync(request.ProductId);

        var stock = warehouse.GetStockOrDefault(product);

        if (stock is null)
            return null;

        return new StockSnapshot(
            product,
            stock.Quantity,
            stock.Price);
    }
}
