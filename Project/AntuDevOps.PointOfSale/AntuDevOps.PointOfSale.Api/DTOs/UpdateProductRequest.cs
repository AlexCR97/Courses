using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record UpdateProductRequest(
    string Code,
    string? DisplayName)
{
    internal UpdateProductCommand ToCommand(int productId, string lastModifiedBy)
    {
        return new UpdateProductCommand(
            new ProductId(productId),
            Code,
            DisplayName,
            lastModifiedBy);
    }
}
