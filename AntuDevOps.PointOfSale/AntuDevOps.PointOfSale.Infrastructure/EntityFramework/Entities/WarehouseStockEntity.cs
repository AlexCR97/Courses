namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class WarehouseStockEntity
{
    public WarehouseStockEntity()
    {
    }

    public WarehouseStockEntity(int warehouseId, WarehouseEntity? warehouse, int productId, ProductEntity? product, int quantity, decimal price)
    {
        WarehouseId = warehouseId;
        Warehouse = warehouse;
        ProductId = productId;
        Product = product;
        Quantity = quantity;
        Price = price;
    }

    public int WarehouseId { get; set; }
    public WarehouseEntity? Warehouse { get; set; }

    public int ProductId { get; set; }
    public ProductEntity? Product { get; set; }

    public int Quantity { get; set; }
    public decimal Price { get; set; }

    public static WarehouseStockEntity Create(int warehouseId, int productId, int quantity, decimal price)
    {
        return new WarehouseStockEntity(
            warehouseId,
            null,
            productId,
            null,
            quantity,
            price);
    }

    public void Update(int quantity, decimal price)
    {
        Quantity = quantity;
        Price = price;
    }
}
