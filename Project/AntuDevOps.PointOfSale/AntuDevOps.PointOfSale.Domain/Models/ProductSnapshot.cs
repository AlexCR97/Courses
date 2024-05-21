namespace AntuDevOps.PointOfSale.Domain.Models;

public class ProductSnapshot
{
    public ProductSnapshot(ProductId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, string code, string displayName, decimal price)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        LastModifiedAt = lastModifiedAt;
        LastModifiedBy = lastModifiedBy;
        Code = code;
        DisplayName = displayName;
        Price = price;
    }

    public ProductId Id { get; }
    public DateTime CreatedAt { get; }
    public string CreatedBy { get; }
    public DateTime? LastModifiedAt { get; }
    public string? LastModifiedBy { get; }
    public string Code { get; }
    public string DisplayName { get; }
    public decimal Price { get; }
}
