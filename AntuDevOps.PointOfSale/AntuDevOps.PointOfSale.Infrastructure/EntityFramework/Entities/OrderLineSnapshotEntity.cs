using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class OrderLineSnapshotEntity
{
    public OrderLineSnapshotEntity()
    {
    }

    public OrderLineSnapshotEntity(ProductSnapshotEntity product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public ProductSnapshotEntity Product { get; set; }
    public int Quantity { get; set; }
}

internal static class OrderLineSnapshotEntityExtensions
{
    public static OrderLineSnapshotEntity ToEntity(this OrderLineSnapshot a)
    {
        return new OrderLineSnapshotEntity(
            a.Product.ToEntity(),
            a.Quantity);
    }

    public static OrderLineSnapshot ToModel(this OrderLineSnapshotEntity a)
    {
        return new OrderLineSnapshot(
            a.Product.ToModel(),
            a.Quantity);
    }
}
