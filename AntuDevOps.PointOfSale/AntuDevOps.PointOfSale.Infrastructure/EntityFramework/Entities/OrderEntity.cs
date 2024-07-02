using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class OrderEntity : BaseEntity
{
    public OrderEntity()
    {
    }

    public OrderEntity(
        int id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, int tenantId, TenantEntity? tenant, string status)
        : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        TenantId = tenantId;
        Tenant = tenant;
        Status = status;
    }

    public int TenantId { get; set; }
    public TenantEntity? Tenant { get; set; }

    public string Status { get; set; }
}

internal static class OrderEntityExtensions
{
    public static OrderEntity ToEntity(this Order a)
    {
        return new OrderEntity(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId.Value,
            null,
            a.Status.ToString());
    }

    public static Order ToModel(this OrderEntity a, IEnumerable<OrderLine> lines)
    {
        return new Order(
            new OrderId(a.Id),
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            new TenantId(a.TenantId),
            Enum.Parse<OrderStatus>(a.Status),
            lines.ToList());
    }
}
