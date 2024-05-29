using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Products;

public record FindProductsQuery(
    TenantId TenantId,
    int Page = FindQuery.PageDefault,
    int Size = FindQuery.SizeDefault,
    Sort? Sort = null,
    string? Search = null)
    : FindQuery(Page, Size, Sort, Search)
    , IRequest<PagedResult<Product>>;

internal class FindProductsQueryHandler : IRequestHandler<FindProductsQuery, PagedResult<Product>>
{
    private readonly IProductRepository _productRepository;

    public FindProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResult<Product>> Handle(FindProductsQuery request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        request = request with
        {
            Search = new SearchExpression(request.Search)
                .And($"tenantId == {request.TenantId.Value}")
                .Build(),
        };

        return await _productRepository.FindAsync(request, cancellationToken);
    }
}
