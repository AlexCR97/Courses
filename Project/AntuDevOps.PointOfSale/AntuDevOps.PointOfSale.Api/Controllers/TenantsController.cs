using AntuDevOps.PointOfSale.Api.DTOs;
using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AntuDevOps.PointOfSale.Api.Controllers;

[ApiController]
[Route("tenants")]
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
    public async Task<ProductId> CreateProduct(
        [FromRoute] int tenantId,
        [FromBody] CreateProductRequest request)
    {
        var productId = await _sender.Send(request.ToCommand(
            "Unknown",
            tenantId));

        return productId;
    }

    [HttpGet("{tenantId:int}/products")]
    public async Task<IReadOnlyList<ProductListResponse>> FindProducts([FromRoute] int tenantId)
    {
        var products = await _sender.Send(new FindProductsQuery(
            // TODO use tenantId
            ));

        return products
            .Select(x => x.ToListResponse())
            .ToList();
    }

    [HttpGet("{tenantId:int}/products/{productId:int}")]
    public async Task<ProductProfileResponse> GetProduct(
        [FromRoute] int tenantId,
        [FromRoute] int productId)
    {
        var product = await _sender.Send(new GetProductQuery(new ProductId(
            //tenantId, // TODO Use tenantId
            productId)));

        return product.ToProfileResponse();
    }

    [HttpPut("{tenantId:int}/products/{productId:int}")]
    public async Task UpdateProduct(
        [FromRoute] int tenantId,
        [FromRoute] int productId,
        [FromBody] UpdateProductRequest request)
    {
        // TODO Verify that product belongs to the tenant

        await _sender.Send(request.ToCommand(
            "Unknown",
            productId));
    }

    [HttpDelete("{tenantId:int}/products/{productId:int}")]
    public async Task DeleteProduct([FromRoute] int tenantId, [FromRoute] int productId)
    {
        // TODO Verify that product belongs to the tenant

        await _sender.Send(new DeleteProductCommand(new ProductId(productId)));
    }

    [HttpPost("{tenantId:int}/products/{productId:int}/images")]
    public Task UploadProductImage(
        [FromRoute] int tenantId,
        [FromRoute] int productId,
        [FromForm] IFormFile file)
    {
        return Task.CompletedTask;
    }

    #endregion
}
