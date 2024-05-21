using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Products;

public record UpdateProductCommand(
    ProductId ProductId,
    string Code,
    string? DisplayName,
    string LastModifiedBy)
    : IRequest;

internal class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
    }
}

internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        new UpdateProductCommandValidator().ValidateAndThrow(request);

        var product = await _productRepository.GetAsync(request.ProductId);

        product.Update(
            request.Code,
            request.DisplayName,
            request.LastModifiedBy);

        await _productRepository.UpdateAsync(product, cancellationToken);
    }
}
