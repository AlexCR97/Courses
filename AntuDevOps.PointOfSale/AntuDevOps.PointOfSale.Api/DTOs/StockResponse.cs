using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record StockResponse(
    StockProductResponse Product,
    int Quantity,
    decimal Price);

internal static class StockResponseExtensions
{
    public static StockResponse ToResponse(this Stock a)
    {
        return new StockResponse(
            a.Product.ToResponse(),
            a.Quantity,
            a.Price);
    }
}
