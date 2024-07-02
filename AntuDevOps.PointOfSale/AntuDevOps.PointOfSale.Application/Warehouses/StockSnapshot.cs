using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Application.Warehouses;

public record StockSnapshot(
    Product Product,
    int Quantity,
    decimal Price)
{
    public bool OutOfStock => Quantity <= 0;

    public bool HasCapacity(int quantity)
    {
        var currentQuantity = Quantity;
        var remainingQuantity = currentQuantity - quantity;
        return remainingQuantity >= 0;
    }
}
