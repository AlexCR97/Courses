using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Products;

public record CreateProductCommand(
    TenantId TenantId,
    string Code,
    string? DisplayName,
    string CreatedBy)
    : IRequest<ProductId>;

internal class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
    }
}

internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductId>
{
    private readonly ITenantRepository _tenantRepository;
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(ITenantRepository tenantRepository, IProductRepository productRepository)
    {
        _tenantRepository = tenantRepository;
        _productRepository = productRepository;
    }

    public async Task<ProductId> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        new CreateProductCommandValidator().ValidateAndThrow(request);

        var tenant = await _tenantRepository.GetAsync(request.TenantId);

        var product = Product.Create(
            tenant,
            request.Code,
            request.DisplayName,
            request.CreatedBy);

        return await _productRepository.CreateAsync(product, cancellationToken);
    }
}
