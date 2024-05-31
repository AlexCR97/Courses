using AntuDevOps.PointOfSale.Api.DTOs;
using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
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
    public async Task<CreatedResult> SignUp([FromBody] SignUpRequest request)
    {
        var result = await _sender.Send(request.ToCommand());
        var response = result.ToResponse();
        return Created((string?)null, response);
    }

    #endregion

    #region Products

    [HttpPost("{tenantId:int}/products")]
    public async Task<CreatedAtActionResult> CreateProduct(
        [FromRoute] int tenantId,
        [FromBody] CreateProductRequest request)
    {
        var productId = await _sender.Send(request.ToCommand(
            "Unknown",
            tenantId));

        return CreatedAtAction(
            nameof(GetProduct),
            new { tenantId, productId = productId.Value },
            new { ProductId = productId.Value });
    }

    [HttpGet("{tenantId:int}/products/v1")]
    public async Task<OkObjectResult> FindProductsV1(
        [FromRoute] int tenantId,
        [FromQuery] int page = FindQuery.PageDefault,
        [FromQuery] int size = FindQuery.SizeDefault,
        [FromQuery] string? sort = null,
        [FromQuery] string? search = null)
    {
        var products = await _sender.Send(new FindProductsQuery(
            new TenantId(tenantId),
            page,
            size,
            Sort.ParseOrDefault(sort),
            search));

        var response = products.Map(x => x.ToListResponse());

        return Ok(response);
    }

    [HttpGet("{tenantId:int}/products/v2")]
    public async Task<OkObjectResult> FindProductsV2(
        [FromRoute] int tenantId,
        [FromQuery] int page = FindQuery.PageDefault,
        [FromQuery] int size = FindQuery.SizeDefault,
        [FromQuery] string? sort = null,
        [FromQuery] string? code = null,
        [FromQuery] string? displayName = null)
    {
        var products = await _sender.Send(new FindProductsQuery(
            new TenantId(tenantId),
            page,
            size,
            Sort.ParseOrDefault(sort),
            new OrExpression()
                .Or(ContainsExpression.For("code", code))
                .Or(ContainsExpression.For("displayName", displayName))
                .BuildExpression()));

        var response = products.Map(x => x.ToListResponse());

        return Ok(response);
    }

    [HttpGet("{tenantId:int}/products/v3")]
    public async Task<OkObjectResult> FindProductsV3(
        [FromRoute] int tenantId,
        [FromQuery] int page = FindQuery.PageDefault,
        [FromQuery] int size = FindQuery.SizeDefault,
        [FromQuery] string? sort = null,
        [FromQuery] string? search = null)
    {
        var products = await _sender.Send(new FindProductsQuery(
            new TenantId(tenantId),
            page,
            size,
            Sort.ParseOrDefault(sort),
            new OrExpression()
                .Or(ContainsExpression.For("code", search))
                .Or(ContainsExpression.For("displayName", search))
                .BuildExpression()));

        var response = products.Map(x => x.ToListResponse());

        return Ok(response);
    }

    [HttpGet("{tenantId:int}/products/{productId:int}", Name = nameof(GetProduct))]
    public async Task<OkObjectResult> GetProduct(
        [FromRoute] int tenantId,
        [FromRoute] int productId)
    {
        var product = await _sender.Send(new GetProductQuery(new ProductId(
            //tenantId, // TODO Use tenantId
            productId)));

        var response = product.ToProfileResponse();

        return Ok(response);
    }

    [HttpPut("{tenantId:int}/products/{productId:int}")]
    public async Task<NoContentResult> UpdateProduct(
        [FromRoute] int tenantId,
        [FromRoute] int productId,
        [FromBody] UpdateProductRequest request)
    {
        // TODO Verify that product belongs to the tenant

        await _sender.Send(request.ToCommand(
            "Unknown",
            productId));

        return NoContent();
    }

    [HttpDelete("{tenantId:int}/products/{productId:int}")]
    public async Task<NoContentResult> DeleteProduct([FromRoute] int tenantId, [FromRoute] int productId)
    {
        // TODO Verify that product belongs to the tenant

        await _sender.Send(new DeleteProductCommand(new ProductId(productId)));

        return NoContent();
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
