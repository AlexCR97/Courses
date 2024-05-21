using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Products;

public record FindProductsQuery
    : IRequest<IReadOnlyList<Product>>;

internal class FindProductsQueryValidator : AbstractValidator<FindProductsQuery>
{
    public FindProductsQueryValidator()
    {
    }
}

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

        new FindProductsQueryValidator().ValidateAndThrow(request);

        return await _productRepository.FindAsync(cancellationToken);
    }
}
