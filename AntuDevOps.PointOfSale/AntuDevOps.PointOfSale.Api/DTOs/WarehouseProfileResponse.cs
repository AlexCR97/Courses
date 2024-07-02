using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record WarehouseProfileResponse(
    int Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? LastModifiedAt,
    string? LastModifiedBy,
    int TenantId,
    string Code,
    string? DisplayName,
    StockResponse[] Stock);

internal static class WarehouseProfileResponseExtensions
{
    public static WarehouseProfileResponse ToProfileResponse(this Warehouse a)
    {
        return new WarehouseProfileResponse(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId.Value,
            a.Code,
            a.DisplayName,
            a.Stock
                .Select(x => x.ToResponse())
                .ToArray());
    }
}