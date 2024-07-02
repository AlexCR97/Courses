using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record OrderProfileResponse(
    int Id,
    DateTime CreatedAt,
    string CreatedBy,
    DateTime? LastModifiedAt,
    string? LastModifiedBy,
    int TenantId,
    string Status,
    OrderLineResponse[] Lines);

internal static class OrderProfileResponseExtensions
{
    public static OrderProfileResponse ToProfileResponse(this Order a)
    {
        return new OrderProfileResponse(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId.Value,
            a.Status.ToString(),
            a.Lines
                .Select(x => x.ToResponse())
                .ToArray());
    }
}