using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class ProductEntity : BaseEntity
{
    public ProductEntity()
    {
    }

    public ProductEntity(int id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, int tenantId, string code, string? displayName) : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        TenantId = tenantId;
        Code = code;
        DisplayName = displayName;
    }

    public int TenantId { get; set; }
    public string Code { get; set; }
    public string? DisplayName { get; set; }
}

internal static class ProductEntityExtensions
{
    public static ProductEntity ToEntity(this Product a)
    {
        return new ProductEntity(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.TenantId.Value,
            a.Code,
            a.DisplayName);
    }

    public static Product ToModel(this ProductEntity a)
    {
        return new Product(
            new ProductId(a.Id),
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            new TenantId(a.TenantId),
            a.Code,
            a.DisplayName);
    }
}
