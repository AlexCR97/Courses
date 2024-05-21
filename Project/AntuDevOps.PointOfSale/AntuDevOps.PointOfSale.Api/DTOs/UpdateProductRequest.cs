using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record UpdateProductRequest(
    int ProductId,
    string Code,
    string? DisplayName)
{
    internal UpdateProductCommand ToCommand(string lastModifiedBy)
    {
        return new UpdateProductCommand(
            new ProductId(ProductId),
            Code,
            DisplayName,
            lastModifiedBy);
    }
}
