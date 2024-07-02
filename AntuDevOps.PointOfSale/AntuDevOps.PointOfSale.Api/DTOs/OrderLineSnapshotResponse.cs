using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record OrderLineSnapshotResponse(
    OrderLineSnapshotProductResponse Product,
    int Quantity);

public record OrderLineSnapshotProductResponse(
    int? ProductId,
    string Code,
    string? DisplayName);

internal static class OrderLineSnapshotResponseExtensions
{
    public static OrderLineSnapshotResponse ToResponse(this OrderLineSnapshot a)
    {
        return new OrderLineSnapshotResponse(
            new OrderLineSnapshotProductResponse(
                a.Product.Id.Value,
                a.Product.Code,
                a.Product.DisplayName),
            a.Quantity);
    }
}
