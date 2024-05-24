using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record ProductProfileResponse(
    int Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? LastModifiedAt,
    string? LastModifiedBy,
    int TenantId,
    string Code,
    string? DisplayName);

internal static class ProductProfileResponseExtensions
{
    public static ProductProfileResponse ToProfileResponse(this Product model)
    {
        return new ProductProfileResponse(
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
