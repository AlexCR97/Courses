using AntuDevOps.PointOfSale.Api.DTOs;
using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AntuDevOps.PointOfSale.Api.Controllers;

[ApiController]
[Route("api/tenants")]
public class TenantsController : ControllerBase
{
    private readonly ISender _sender;

    public TenantsController(ISender sender)
    {
        _sender = sender;
    }

    #region Tenant

    [HttpPost("sign-up")]
    public async Task<SignedUpResponse> SignUp([FromBody] SignUpRequest request)
    {
        var result = await _sender.Send(request.ToCommand());
        return result.ToResponse();
    }

    #endregion

    #region Products

    [HttpPost("{tenantId:int}/products")]
    public async Task<ProductId> CreateProduct([FromRoute] int tenantId, [FromBody] CreateProductRequest request)
    {
        var productId = await _sender.Send(request.ToCommand(
            tenantId,
            "Unknown" // TODO Pass username
            ));

        return productId;
    }

    [HttpGet("{tenantId:int}/products")]
    public async Task<IReadOnlyList<ProductListResponse>> FindProducts([FromRoute] int tenantId)
    {
        // TODO Use tenantId to filter results

        var products = await _sender.Send(new FindProductsQuery());

        return products
            .Select(x => x.ToResponse())
            .ToList();
    }

    [HttpPut("{tenantId:int}/products/{productId:int}")]
    public async Task UpdateProduct([FromRoute] int tenantId, [FromRoute] int productId, [FromBody] UpdateProductRequest request)
    {
        // TODO Use tenantId to verify that Product belongs to Tenant

        await _sender.Send(request.ToCommand(
            productId,
            "Unknown" // TODO Pass username
            ));
    }

    [HttpDelete("{tenantId:int}/products/{productId:int}")]
    public async Task DeleteProduct([FromRoute] int tenantId, [FromRoute] int productId)
    {
        // TODO Use tenantId to verify that Product belongs to Tenant

        await _sender.Send(new DeleteProductCommand(new ProductId(productId)));
    }

    #endregion
}
