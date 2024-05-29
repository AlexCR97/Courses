using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Application.Users;
using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Seeding;

public record SeedCommand(
    string SeededBy)
    : IRequest;

internal class SeedCommandHandler : IRequestHandler<SeedCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ISender _sender;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;

    public SeedCommandHandler(IProductRepository productRepository, ISender sender, ITenantRepository tenantRepository, IUserRepository userRepository)
    {
        _productRepository = productRepository;
        _sender = sender;
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(SeedCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var globalTenantResult = await CreateUserAsync("Global Tenant", new Email("admin@global.com"), cancellationToken);
        await CreateProductsAsync(globalTenantResult.TenantId, "GT", request.SeededBy, cancellationToken);

        var antuDevOpsTenantResult = await CreateUserAsync("Antu DevOps", new Email("pablo.castillo@antudevops.com"), cancellationToken);
        await CreateProductsAsync(antuDevOpsTenantResult.TenantId, "ADO", request.SeededBy, cancellationToken);
    }

    private async Task<(TenantId TenantId, UserId UserId)> CreateUserAsync(string tenantName, Email userEmail, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var tenant = await _tenantRepository.GetByNameOrDefaultAsync(tenantName, cancellationToken);

        var user = await _userRepository.GetByEmailOrDefaultAsync(userEmail, cancellationToken);

        if (tenant is not null && user is not null)
            return (tenant.Id, user.Id);

        var result = await _sender.Send(
            new SignUpCommand(
                tenantName,
                UserTenantRole.Owner,
                userEmail,
                new Password("123Qwe"),
                "Admin",
                "User"),
            cancellationToken);

        return (result.TenantId, result.UserId);
    }

    private async Task CreateProductsAsync(TenantId tenantId, string codePrefix, string seededBy, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var productCodes = Enumerable
            .Range(1, 100)
            .Select(index => $"{codePrefix}-{index.ToString().PadLeft(3, '0')}");

        foreach (var code in productCodes)
        {
            var product = await _productRepository.GetByCodeOrDefaultAsync(code, cancellationToken);

            if (product is not null)
                continue;

            await _sender.Send(new CreateProductCommand(
                tenantId,
                code,
                null,
                seededBy));
        }
    }
}
