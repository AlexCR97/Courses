using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record ProductListResponse(
    int Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? LastModifiedAt,
    string? LastModifiedBy,
    int TenantId,
    string Code,
    string? DisplayName);

internal static class ProductListResponseExtensions
{
    public static ProductListResponse ToListResponse(this Product model)
    {
        return new ProductListResponse(
            model.Id.Value,
            model.CreatedAt,
            model.CreatedBy,
            model.LastModifiedAt,
            model.LastModifiedBy,
            model.TenantId.Value,
            model.Code,
            model.DisplayName);
    }
}
