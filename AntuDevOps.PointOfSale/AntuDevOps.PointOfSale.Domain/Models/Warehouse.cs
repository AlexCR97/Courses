namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly record struct WarehouseId(int Value);

public class Warehouse : AggregateRoot<WarehouseId>
{
    public Warehouse(WarehouseId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy,
        TenantId tenantId, string code, string displayName, List<Stock> stock)
        : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        TenantId = tenantId;
        Code = code;
        DisplayName = displayName;
        Stock = stock;
    }

    public TenantId TenantId { get; }
    public string Code { get; private set; }
    public string DisplayName { get; private set; }
    public List<Stock> Stock { get; }

    public static Warehouse Create(Tenant tenant, string createdBy, string code, string displayName)
    {
        return new Warehouse(
            default,
            DateTime.UtcNow,
            createdBy,
            null,
            null,
            tenant.Id,
            code,
            displayName,
            new List<Stock>());
    }

    public void AddStock(Product product, int quantity, decimal price)
    {
        Stock.Add(new Stock(
            new StockProduct(
                product.Id,
                product.Code,
                product.DisplayName),
            quantity,
            price));
    }

    public void AddStockQuantity(Product product, int quantity)
    {
        this
            .GetStock(product)
            .AddQuantity(quantity);
    }

    public Stock GetStock(Product product)
    {
        return GetStockOrDefault(product)
            ?? throw new Exception("No such product in stock");
    }

    public Stock? GetStockOrDefault(Product product)
    {
        return Stock.Find(x => x.Product.ProductId == product.Id);
    }

    public bool RemoveStock(Product product)
    {
        var stock = GetStockOrDefault(product);

        if (stock is null)
            return false;

        Stock.Remove(stock);
        return true;
    }

    public bool RemoveStockQuantity(Product product, int quantity)
    {
        return this
            .GetStock(product)
            .RemoveQuantity(quantity);
    }
}

public class Stock
{
    public Stock(StockProduct product, int quantity, decimal price)
    {
        Product = product;
        Quantity = quantity;
        Price = price;
    }

    public StockProduct Product { get; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public void AddQuantity(int quantity)
    {
        Quantity += quantity;
    }

    public bool RemoveQuantity(int quantity)
    {
        Quantity = Math.Min(0, Quantity - quantity);
        return Quantity <= 0;
    }
}

public record StockProduct(
    ProductId ProductId,
    string Code,
    string? DisplayName);
