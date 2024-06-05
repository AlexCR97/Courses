using AntuDevOps.PointOfSale.Api.DTOs;
using AntuDevOps.PointOfSale.Application.Orders;
using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Application.Warehouses;
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
            new { productId = productId.Value });
    }

    [HttpGet("{tenantId:int}/products")]
    public async Task<OkObjectResult> FindProducts(
        [FromRoute] int tenantId,
        [FromQuery] int page = FindQuery.PageDefault,
        [FromQuery] int size = FindQuery.SizeDefault,
        [FromQuery] string? sort = null)
    {
        var products = await _sender.Send(new FindProductsQuery(
            new TenantId(tenantId),
            page,
            size,
            Sort.ParseOrDefault(sort)));

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

    #region Warehouses

    [HttpPost("{tenantId:int}/warehouses")]
    public async Task<CreatedResult> CreateWarehouse(
        [FromRoute] int tenantId,
        [FromBody] CreateWarehouseRequest request)
    {
        var warehouseId = await _sender.Send(new CreateWarehouseCommand(
            new TenantId(tenantId),
            request.Code,
            request.DisplayName,
            "Unknown"));

        return Created((string?)null, new { warehouseId = warehouseId.Value });
    }

    [HttpGet("{tenantId:int}/warehouses")]
    public async Task<OkObjectResult> FindWarehouses(
        [FromRoute] int tenantId,
        [FromQuery] int page = FindQuery.PageDefault,
        [FromQuery] int size = FindQuery.SizeDefault,
        [FromQuery] string? sort = null)
    {
        // TODO Use tenantId

        var warehouses = await _sender.Send(new FindWarehousesQuery(
            new TenantId(tenantId),
            page,
            size,
            Sort.ParseOrDefault(sort)));

        var response = warehouses.Map(x => x.ToListResponse());

        return Ok(response);
    }

    [HttpGet("{tenantId:int}/warehouses/{warehouseId:int}")]
    public async Task<OkObjectResult> GetWarehouse(
        [FromRoute] int tenantId,
        [FromRoute] int warehouseId)
    {
        // TODO Use tenantId

        var warehouse = await _sender.Send(new GetWarehouseQuery(new WarehouseId(warehouseId)));

        var response = warehouse.ToProfileResponse();

        return Ok(response);
    }

    [HttpPost("{tenantId:int}/warehouses/{warehouseId:int}/stock")]
    public async Task<NoContentResult> AddStock(
        [FromRoute] int tenantId,
        [FromRoute] int warehouseId,
        [FromBody] AddStockRequest request)
    {
        // TODO Use tenantId

        await _sender.Send(new AddStockCommand(
            new WarehouseId(warehouseId),
            new ProductId(request.ProductId),
            request.Quantity,
            request.Price));

        return NoContent();
    }

    [HttpPost("{tenantId:int}/warehouses/{warehouseId:int}/stock/{productId:int}/quantity")]
    public async Task<NoContentResult> AddStockQuantity(
        [FromRoute] int tenantId,
        [FromRoute] int warehouseId,
        [FromRoute] int productId,
        [FromBody] AddStockQuantityRequest request)
    {
        // TODO Use tenantId

        await _sender.Send(new AddStockQuantityCommand(
            new WarehouseId(warehouseId),
            new ProductId(productId),
            request.Quantity));

        return NoContent();
    }

    [HttpDelete("{tenantId:int}/warehouses/{warehouseId:int}/stock/{productId:int}")]
    public async Task<NoContentResult> RemoveStock(
        [FromRoute] int tenantId,
        [FromRoute] int warehouseId,
        [FromRoute] int productId)
    {
        // TODO Use tenantId

        await _sender.Send(new RemoveStockCommand(
            new WarehouseId(warehouseId),
            new ProductId(productId)));

        return NoContent();
    }

    [HttpDelete("{tenantId:int}/warehouses/{warehouseId:int}/stock/{productId:int}/quantity")]
    public async Task<OkObjectResult> RemoveStockQuantity(
        [FromRoute] int tenantId,
        [FromRoute] int warehouseId,
        [FromRoute] int productId,
        [FromBody] RemoveStockQuantityRequest request)
    {
        // TODO Use tenantId

        var result = await _sender.Send(new RemoveStockQuantityCommand(
            new WarehouseId(warehouseId),
            new ProductId(productId),
            request.Quantity));

        var response = new RemoveStockQuantityResponse(result.OutOfStock);

        return Ok(response);
    }

    #endregion

    #region Orders

    [HttpPost("{tenantId:int}/orders")]
    public async Task<CreatedResult> CreateOrder(
        [FromRoute] int tenantId,
        [FromBody] CreateOrderRequest request)
    {
        var createOrderCommand = request.ToCommand(tenantId, "Unknown");
        var orderId = await _sender.Send(createOrderCommand);
        return Created((string?)null, new { orderId = orderId.Value });
    }

    [HttpGet("{tenantId:int}/orders")]
    public async Task<OkObjectResult> FindOrders(
        [FromRoute] int tenantId,
        [FromQuery] int page = FindQuery.PageDefault,
        [FromQuery] int size = FindQuery.SizeDefault,
        [FromQuery] string? sort = null)
    {
        // TODO Use tenantId

        var orders = await _sender.Send(new FindOrdersQuery(
            new TenantId(tenantId),
            page,
            size,
            Sort.ParseOrDefault(sort)));

        var response = orders.Map(x => x.ToListResponse());

        return Ok(response);
    }

    [HttpGet("{tenantId:int}/orders/{orderId:int}")]
    public async Task<OkObjectResult> GetOrder(
        [FromRoute] int tenantId,
        [FromRoute] int orderId)
    {
        // TODO Use tenantId

        var order = await _sender.Send(new GetOrderQuery(new OrderId(orderId)));

        var response = order.ToProfileResponse();

        return Ok(response);
    }

    [HttpPost("{tenantId:int}/orders/{orderId:int}/lines/{productId:int}/quantity")]
    public async Task<NoContentResult> IncreaseOrderProductQuantity(
        [FromRoute] int tenantId,
        [FromRoute] int orderId,
        [FromRoute] int productId,
        [FromBody] IncreaseOrderProductQuantityRequest request)
    {
        // TODO Use tenantId

        await _sender.Send(new IncreaseOrderProductQuantityCommand(
            new OrderId(orderId),
            new WarehouseId(request.WarehouseId),
            new ProductId(productId),
            "Unknown"));

        return NoContent();
    }

    [HttpDelete("{tenantId:int}/orders/{orderId:int}/lines/{productId:int}/quantity")]
    public async Task<NoContentResult> DecreaseOrderProductQuantity(
        [FromRoute] int tenantId,
        [FromRoute] int orderId,
        [FromRoute] int productId,
        [FromBody] DecreaseOrderProductQuantityRequest request)
    {
        // TODO Use tenantId

        await _sender.Send(new DecreaseOrderProductQuantityCommand(
            new OrderId(orderId),
            new WarehouseId(request.WarehouseId),
            new ProductId(productId),
            "Unknown"));

        return NoContent();
    }

    #endregion
}
