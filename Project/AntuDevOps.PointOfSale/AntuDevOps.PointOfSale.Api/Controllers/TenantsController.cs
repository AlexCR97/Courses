using AntuDevOps.PointOfSale.Api.DTOs;
using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AntuDevOps.PointOfSale.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class TenantController : ControllerBase
{
    private readonly ISender _sender;

    public TenantController(ISender sender)
    {
        _sender = sender;
    }

    #region Tenant

    [HttpPost("SignUp")]
    public async Task<SignedUpResponse> SignUp([FromBody] SignUpRequest request)
    {
        var result = await _sender.Send(request.ToCommand());
        return result.ToResponse();
    }

    #endregion

    #region Products

    [HttpPost("addProduct")]
    public async Task<ProductId> CreateProduct([FromBody] CreateProductRequest request)
    {
        var productId = await _sender.Send(request.ToCommand("Unknown"));
        return productId;
    }

    [HttpGet("{tenantId:int}/products/list")]
    public async Task<IReadOnlyList<ProductListResponse>> FindProducts()
    {
        var products = await _sender.Send(new FindProductsQuery());

        return products
            .Select(x => x.ToListResponse())
            .ToList();
    }

    [HttpGet("{tenantId:int}/product/{productId:int}")]
    public async Task<ProductProfileResponse> GetProduct(int productId)
    {
        var product = await _sender.Send(new GetProductQuery(new ProductId(productId)));

        return product.ToProfileResponse();
    }

    [HttpPost("update_product")]
    public async Task UpdateProduct([FromBody] UpdateProductRequest request)
    {
        await _sender.Send(request.ToCommand("Unknown"));
    }

    [HttpDelete("{tenantId:int}/{productId:int}/products")]
    public async Task DeleteProduct([FromRoute] int tenantId, [FromRoute] int productId)
    {
        await _sender.Send(new DeleteProductCommand(new ProductId(productId)));
    }

    [HttpPut("product/{productId:int}/upload-image")]
    public Task UploadProductImage([FromForm] IFormFile file)
    {
        return Task.CompletedTask;
    }

    #endregion
}
