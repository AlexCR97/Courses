namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly record struct OrderId(int Value);

public class Order : AggregateRoot<OrderId>
{
    public Order(OrderId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, UserId userId, OrderStatus status, List<OrderLine> lines) : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        UserId = userId;
        Status = status;
        Lines = lines;
    }

    public UserId UserId { get; }
    public OrderStatus Status { get; }
    public List<OrderLine> Lines { get; }
}

public enum OrderStatus
{
    Pending,
    Processing,
    Cancelled,
    Failed,
    Completed,
}

public class OrderLine
{
    public OrderLine(OrderLineProduct product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public OrderLineProduct Product { get; }
    public int Quantity { get; }
}

public record OrderLineProduct(
    int Id,
    string Code,
    string DisplayName);
