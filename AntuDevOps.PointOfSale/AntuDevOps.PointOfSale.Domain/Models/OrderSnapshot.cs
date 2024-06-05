namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly record struct OrderSnapshotId(int Value);

public class OrderSnapshot : AggregateRoot<OrderSnapshotId>
{
    public OrderSnapshot(OrderSnapshotId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, TenantId? tenantId, OrderId? orderId, OrderStatus status, IReadOnlyList<OrderLineSnapshot> lines)
        : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        TenantId = tenantId;
        OrderId = orderId;
        Status = status;
        Lines = lines;
    }

    /// <summary>
    /// The original Tenant could be deleted, so we need the TenantId to be nullable.
    /// </summary>
    public TenantId? TenantId { get; private set; }

    /// <summary>
    /// The original Order could be deleted, so we need the OrderId to be nullable.
    /// </summary>
    public OrderId? OrderId { get; private set; }
    public OrderStatus Status { get; }
    public IReadOnlyList<OrderLineSnapshot> Lines { get; }

    public static OrderSnapshot Create(TenantId tenantId, OrderId orderId, OrderStatus status, string createdBy, IEnumerable<OrderLineSnapshot> lines)
    {
        return new OrderSnapshot(
            default,
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            tenantId,
            orderId,
            status,
            lines.ToList());
    }
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
