namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly record struct WarehouseId(int Value);

public class Warehouse : AggregateRoot<WarehouseId>
{
    public Warehouse(WarehouseId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, string code, string displayName, List<Stock> stock) : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        Code = code;
        DisplayName = displayName;
        Stock = stock;
    }

    public string Code { get; private set; }
    public string DisplayName { get; private set; }
    public List<Stock> Stock { get; }
}

public class Stock
{
    public Stock(Product product, int quantity, decimal price)
    {
        Product = product;
        Quantity = quantity;
        Price = price;
    }

    public Product Product { get; private set; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }
}
