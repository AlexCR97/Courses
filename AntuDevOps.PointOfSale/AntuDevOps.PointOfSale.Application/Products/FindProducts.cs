using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Products;

public record FindProductsQuery(
    TenantId TenantId)
    : IRequest<IReadOnlyList<Product>>;

internal class FindProductsQueryHandler : IRequestHandler<FindProductsQuery, IReadOnlyList<Product>>
{
    private readonly IProductRepository _productRepository;

    public FindProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IReadOnlyList<Product>> Handle(FindProductsQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await _productRepository.FindAsync(cancellationToken);
    }
}
