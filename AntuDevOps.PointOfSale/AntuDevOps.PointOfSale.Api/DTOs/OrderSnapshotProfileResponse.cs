using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record OrderSnapshotProfileResponse(
    int Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? LastModifiedAt,
    string? LastModifiedBy,
    int? TenantId,
    string Status,
    OrderLineSnapshotResponse[] Lines);

internal static class OrderSnapshotProfileResponseExtensions
{
    public static OrderSnapshotProfileResponse ToProfileResponse(this OrderSnapshot a)
    {
        return new OrderSnapshotProfileResponse(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId?.Value,
            a.Status.ToString(),
            a.Lines
                .Select(x => x.ToResponse())
                .ToArray());
    }
}
