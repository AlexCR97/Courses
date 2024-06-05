using AntuDevOps.PointOfSale.Application.Orders;
using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record DecreaseOrderProductQuantityRequest(
    int WarehouseId,
    int ProductId)
{
    internal DecreaseOrderProductQuantityCommand ToCommand(int orderId, string? lastModifiedBy)
    {
        return new DecreaseOrderProductQuantityCommand(
            new OrderId(orderId),
            new WarehouseId(WarehouseId),
            new ProductId(ProductId),
            lastModifiedBy);
    }
}
