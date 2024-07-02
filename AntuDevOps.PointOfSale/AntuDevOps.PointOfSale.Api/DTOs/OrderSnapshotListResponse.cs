using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record OrderSnapshotListResponse(
    int Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? LastModifiedAt,
    string? LastModifiedBy,
    int? TenantId,
    string Status);

internal static class OrderSnapshotListResponseExtensions
{
    public static OrderSnapshotListResponse ToListResponse(this OrderSnapshot a)
    {
        return new OrderSnapshotListResponse(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId?.Value,
            a.Status.ToString());
    }
}
