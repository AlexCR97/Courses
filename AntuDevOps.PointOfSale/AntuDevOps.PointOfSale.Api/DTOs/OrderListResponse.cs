using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record OrderListResponse(
    int Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? LastModifiedAt,
    string? LastModifiedBy,
    int TenantId,
    string Status);

internal static class OrderListResponseExtensions
{
    public static OrderListResponse ToListResponse(this Order a)
    {
        return new OrderListResponse(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId.Value,
            a.Status.ToString());
    }
}
