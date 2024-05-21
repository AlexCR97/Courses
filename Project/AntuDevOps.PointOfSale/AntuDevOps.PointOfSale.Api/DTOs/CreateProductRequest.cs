using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record CreateProductRequest(
    int TenantId,
    string Code,
    string? DisplayName)
{
    internal CreateProductCommand ToCommand(string createdBy)
    {
        return new CreateProductCommand(
            new TenantId(TenantId),
            Code,
            DisplayName,
            createdBy);
    }
}
