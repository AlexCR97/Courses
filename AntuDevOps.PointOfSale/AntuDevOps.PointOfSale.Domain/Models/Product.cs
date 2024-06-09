namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly record struct ProductId(int Value);

public class Product : AggregateRoot<ProductId>
{
    public Product(ProductId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, TenantId tenantId, string code, string? displayName) : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        TenantId = tenantId;
        Code = code;
        DisplayName = displayName;
    }

    public TenantId TenantId { get; }
    public string Code { get; private set; }
    public string? DisplayName { get; private set; }

    public static Product Create(Tenant tenant, string code, string? displayName, string createdBy)
    {
        return new Product(
            default,
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            tenant.Id,
            code,
            displayName);
    }

    public void Update(string code, string? displayName, string lastModifiedBy)
    {
        LastModifiedAt = DateTime.UtcNow;
        LastModifiedBy = lastModifiedBy;
        Code = code;
        DisplayName = displayName;
    }
}
