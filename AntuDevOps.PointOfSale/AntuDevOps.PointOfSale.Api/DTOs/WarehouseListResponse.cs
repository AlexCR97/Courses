using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record WarehouseListResponse(
    int Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? LastModifiedAt,
    string? LastModifiedBy,
    int TenantId,
    string Code,
    string? DisplayName);

internal static class WarehouseListResponseExtensions
{
    public static WarehouseListResponse ToListResponse(this Warehouse a)
    {
        return new WarehouseListResponse(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId.Value,
            a.Code,
            a.DisplayName);
    }
}