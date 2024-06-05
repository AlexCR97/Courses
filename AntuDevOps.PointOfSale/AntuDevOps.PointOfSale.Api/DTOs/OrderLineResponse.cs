using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record OrderLineResponse(
    OrderLineProductResponse Product,
    int Quantity);

public record OrderLineProductResponse(
    int ProductId,
    string Code,
    string? DisplayName);

internal static class OrderLineResponseExtensions
{
    public static OrderLineResponse ToResponse(this OrderLine a)
    {
        return new OrderLineResponse(
            new OrderLineProductResponse(
                a.Product.ProductId.Value,
                a.Product.Code,
                a.Product.DisplayName),
            a.Quantity);
    }
}
