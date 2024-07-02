namespace AntuDevOps.PointOfSale.Domain.Models;

public class ProductSnapshot
{
    public ProductSnapshot(ProductId id, DateTime createdAt, string createdBy, string code, string? displayName, decimal price)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        Code = code;
        DisplayName = displayName;
        Price = price;
    }

    public ProductId Id { get; }
    public DateTime CreatedAt { get; }
    public string CreatedBy { get; }
    public string Code { get; }
    public string? DisplayName { get; }
    public decimal Price { get; }
}
