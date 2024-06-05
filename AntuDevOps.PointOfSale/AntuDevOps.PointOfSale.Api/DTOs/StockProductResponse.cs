using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record StockProductResponse(
    int ProductId,
    string Code,
    string? DisplayName);

internal static class StockProductResponseExtensions
{
    public static StockProductResponse ToResponse(this StockProduct a)
    {
        return new StockProductResponse(
            a.ProductId.Value,
            a.Code,
            a.DisplayName);
    }
}