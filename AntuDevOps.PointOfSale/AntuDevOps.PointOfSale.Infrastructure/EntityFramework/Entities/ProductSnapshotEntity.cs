using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class ProductSnapshotEntity
{
    public ProductSnapshotEntity()
    {
    }

    public ProductSnapshotEntity(int id, DateTime createdAt, string createdBy, string code, string? displayName, decimal price)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        Code = code;
        DisplayName = displayName;
        Price = price;
    }

    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public string Code { get; set; }
    public string? DisplayName { get; set; }
    public decimal Price { get; set; }
}

internal static class ProductSnapshotEntityExtensions
{
    public static ProductSnapshotEntity ToEntity(this ProductSnapshot a)
    {
        return new ProductSnapshotEntity(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.Code,
            a.DisplayName,
            a.Price);
    }

    public static ProductSnapshot ToModel(this ProductSnapshotEntity a)
    {
        return new ProductSnapshot(
            new ProductId(a.Id),
            a.CreatedAt,
            a.CreatedBy,
            a.Code,
            a.DisplayName,
            a.Price);
    }
}
