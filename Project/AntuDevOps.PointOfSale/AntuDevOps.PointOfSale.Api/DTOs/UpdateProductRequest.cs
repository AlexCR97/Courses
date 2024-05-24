using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record UpdateProductRequest(
    string Code,
    string? DisplayName)
{
    internal UpdateProductCommand ToCommand(string lastModifiedBy, int productId)
    {
        return new UpdateProductCommand(
            new ProductId(productId),
            Code,
            DisplayName,
            lastModifiedBy);
    }
}
