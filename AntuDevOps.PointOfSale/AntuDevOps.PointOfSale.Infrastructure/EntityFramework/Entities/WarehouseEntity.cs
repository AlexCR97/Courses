using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class WarehouseEntity : BaseEntity
{
    public WarehouseEntity()
    {
    }

    public WarehouseEntity(
        int id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, int tenantId, TenantEntity? tenant, string code, string displayName)
        : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        TenantId = tenantId;
        Tenant = tenant;
        Code = code;
        DisplayName = displayName;
    }

    public int TenantId { get; set; }
    public TenantEntity? Tenant { get; set; }

    public string Code { get; set; }
    public string DisplayName { get; set; }
}

internal static class WarehouseEntityExtensions
{
    public static WarehouseEntity ToEntity(this Warehouse a)
    {
        return new WarehouseEntity(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId.Value,
            null,
            a.Code,
            a.DisplayName);
    }

    public static Warehouse ToModel(this WarehouseEntity a, List<Stock> stock)
    {
        return new Warehouse(
            new WarehouseId(a.Id),
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            new TenantId(a.TenantId),
            a.Code,
            a.DisplayName,
            stock);
    }
}
