namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly record struct OrderId(int Value);

public class Order : AggregateRoot<OrderId>
{
    public Order(OrderId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, TenantId tenantId, OrderStatus status, List<OrderLine> lines) : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        TenantId = tenantId;
        Status = status;
        Lines = lines;
    }

    public TenantId TenantId { get; }
    public OrderStatus Status { get; private set; }
    public List<OrderLine> Lines { get; }

    public static Order Create(Tenant tenant, Product product, string createdBy)
    {
        var order = new Order(
            default,
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            tenant.Id,
            OrderStatus.Drafted,
            new List<OrderLine>());

        order.AddProduct(product, createdBy);

        return order;
    }

    public void AddProduct(Product product, string? lastModifiedBy)
    {
        var line = OrderLine.Create(product);
        line.IncreaseQuantity();
        Lines.Add(line);
        LastModifiedBy = lastModifiedBy;
    }

    public void IncreaseProductQuantity(Product product, string? lastModifiedBy)
    {
        this
            .GetLine(product.Id)
            .IncreaseQuantity();

        LastModifiedBy = lastModifiedBy;
    }

    public bool DecreaseProductQuantity(Product product, string? lastModifiedBy)
    {
        var empty = this
            .GetLine(product.Id)
            .DecreaseQuantity();

        LastModifiedBy = lastModifiedBy;

        if (!empty)
            return false;

        return RemoveProduct(product, lastModifiedBy);
    }

    public bool RemoveProduct(Product product, string? lastModifiedBy)
    {
        var line = GetLineOrDefault(product.Id);

        if (line is null)
            return false;

        Lines.Remove(line);
        LastModifiedBy = lastModifiedBy;

        return true;
    }

    public OrderLine GetLine(ProductId productId)
    {
        return GetLineOrDefault(productId)
            ?? throw new Exception($"No such OrderLine with Product {productId.Value}");
    }

    public OrderLine? GetLineOrDefault(ProductId productId)
    {
        return Lines.Find(x => x.Product.ProductId == productId);
    }

    public void SetStatus(OrderStatus status, string? lastModifiedBy)
    {
        if (!Status.CanTransitionTo(status))
            throw new Exception("Invalid transition");

        Status = status;
        LastModifiedBy = lastModifiedBy;
    }
}

public enum OrderStatus
{
    Drafted,
    Processing,
    Cancelled,
    Completed,
}

internal static class OrderStatusStateMachine
{
    private static readonly IReadOnlyDictionary<OrderStatus, IReadOnlySet<OrderStatus>> _transitions = new Dictionary<OrderStatus, IReadOnlySet<OrderStatus>>
    {
        [OrderStatus.Drafted] = new HashSet<OrderStatus>
        {
            OrderStatus.Processing,
            OrderStatus.Cancelled,
        },
        [OrderStatus.Processing] = new HashSet<OrderStatus>
        {
            OrderStatus.Drafted,
            OrderStatus.Cancelled,
            OrderStatus.Completed,
        },
        [OrderStatus.Cancelled] = new HashSet<OrderStatus>
        {
        },
        [OrderStatus.Completed] = new HashSet<OrderStatus>
        {
        },
    };

    public static bool CanTransitionTo(this OrderStatus from, OrderStatus to)
    {
        var transition = _transitions.GetValueOrDefault(from);

        if (transition is null)
            return false;

        return transition.Contains(to);
    }
}

public class OrderLine
{
    public OrderLine(OrderLineProduct product, int quantity)
    {
        Product = product;
        Quantity = quantity;
    }

    public OrderLineProduct Product { get; }
    public int Quantity { get; private set; }

    public static OrderLine Create(Product product)
    {
        return new OrderLine(
            new OrderLineProduct(
                product.Id,
                product.Code,
                product.DisplayName),
            0);
    }

    public void IncreaseQuantity()
    {
        Quantity += 1;
    }

    public bool DecreaseQuantity()
    {
        Quantity = Math.Min(0, Quantity - 1);
        return Quantity == 0;
    }
}

public record OrderLineProduct(
    ProductId ProductId,
    string Code,
    string? DisplayName);
