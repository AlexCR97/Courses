using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record CreateProductRequest(
    string Code,
    string? DisplayName)
{
    internal CreateProductCommand ToCommand(string createdBy, int tenantId)
    {
        return new CreateProductCommand(
            new TenantId(tenantId),
            Code,
            DisplayName,
            createdBy);
    }
}
