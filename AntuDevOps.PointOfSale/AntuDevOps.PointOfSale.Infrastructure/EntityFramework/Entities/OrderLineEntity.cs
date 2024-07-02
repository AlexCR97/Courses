

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class OrderLineEntity
{
    public OrderLineEntity()
    {
    }

    public OrderLineEntity(int orderId, OrderEntity? order, int productId, ProductEntity? product, int quantity)
    {
        OrderId = orderId;
        Order = order;
        ProductId = productId;
        Product = product;
        Quantity = quantity;
    }

    public int OrderId { get; set; }
    public OrderEntity? Order { get; set; }

    public int ProductId { get; set; }
    public ProductEntity? Product { get; set; }

    public int Quantity { get; set; }

    public static OrderLineEntity Create(int orderId, int productId, int quantity)
    {
        return new OrderLineEntity(orderId, null, productId, null, quantity);
    }

    public void Update(int quantity)
    {
        Quantity = quantity;
    }
}
