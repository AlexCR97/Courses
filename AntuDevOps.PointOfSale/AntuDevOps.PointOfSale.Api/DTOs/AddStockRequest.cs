namespace AntuDevOps.PointOfSale.Api.DTOs;

public record AddStockRequest(
    int ProductId,
    int Quantity,
    decimal Price);