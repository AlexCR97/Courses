namespace AntuDevOps.PointOfSale.Domain.Models;

public class OrderSnapshot
{
    public OrderSnapshot(OrderId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, UserSnapshot user, OrderStatus status, List<OrderLineSnapshot> lines)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        LastModifiedAt = lastModifiedAt;
        LastModifiedBy = lastModifiedBy;
        User = user;
        Status = status;
        Lines = lines;
    }

    public OrderId Id { get; }
    public DateTime CreatedAt { get; }
    public string CreatedBy { get; }
    public DateTime? LastModifiedAt { get; }
    public string? LastModifiedBy { get; }
    public UserSnapshot User { get; }
    public OrderStatus Status { get; }
    public List<OrderLineSnapshot> Lines { get; }
}

public class OrderLineSnapshot
{
    public OrderLineSnapshot(ProductSnapshot product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public ProductSnapshot Product { get; }
    public int Quantity { get; }
}
