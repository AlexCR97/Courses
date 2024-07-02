using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class OrderSnapshotEntity : BaseEntity
{
    public OrderSnapshotEntity()
    {
    }

    public OrderSnapshotEntity(int id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, int? tenantId, int? orderId, string status, List<OrderLineSnapshotEntity> lines)
        : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        TenantId = tenantId;
        OrderId = orderId;
        Status = status;
        Lines = lines;
    }

    public int? TenantId { get; set; }
    public int? OrderId { get; set; }
    public string Status { get; set; }
    public List<OrderLineSnapshotEntity> Lines { get; set; }
}

internal static class OrderSnapshotEntityExtensions
{
    public static OrderSnapshotEntity ToEntity(this OrderSnapshot a)
    {
        return new OrderSnapshotEntity(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId?.Value,
            a.OrderId?.Value,
            a.Status.ToString(),
            a.Lines
                .Select(line => line.ToEntity())
                .ToList());
    }

    public static OrderSnapshot ToModel(this OrderSnapshotEntity a)
    {
        return new OrderSnapshot(
            new OrderSnapshotId(a.Id),
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId is null ? null : new TenantId(a.TenantId.Value),
            a.OrderId is null ? null : new OrderId(a.OrderId.Value),
            Enum.Parse<OrderStatus>(a.Status),
            a.Lines
                .Select(line => line.ToModel())
                .ToArray());
    }
}
