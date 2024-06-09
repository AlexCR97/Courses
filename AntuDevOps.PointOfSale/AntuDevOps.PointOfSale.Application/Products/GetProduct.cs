using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Products;

public record GetProductQuery(ProductId ProductId) : IRequest<Product>;

internal class GetProductQueryHandler : IRequestHandler<GetProductQuery, Product>
{
    private readonly IProductRepository _productRepository;

    public GetProductQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _productRepository.GetAsync(request.ProductId, cancellationToken);
    }
}
