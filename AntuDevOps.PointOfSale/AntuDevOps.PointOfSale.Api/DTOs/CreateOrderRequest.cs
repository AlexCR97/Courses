using AntuDevOps.PointOfSale.Application.Orders;
using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record CreateOrderRequest(
    int WarehouseId,
    int ProductId)
{
    internal CreateOrderCommand ToCommand(int tenantId, string createdBy)
    {
        return new CreateOrderCommand(
            new TenantId(tenantId),
            new WarehouseId(WarehouseId),
            new ProductId(ProductId),
            createdBy);
    }
}
