using AntuDevOps.PointOfSale.Application.Orders;
using AntuDevOps.PointOfSale.Application.Products;
using AntuDevOps.PointOfSale.Application.Users;
using AntuDevOps.PointOfSale.Application.Warehouses;
using AntuDevOps.PointOfSale.Domain.Common;
using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using MediatR;

namespace AntuDevOps.PointOfSale.Application.Seeding;

public record SeedCommand() : IRequest;

internal class SeedCommandHandler : IRequestHandler<SeedCommand>
{
    private const string _createdBy = "Seeding";

    private readonly IProductRepository _productRepository;
    private readonly ISender _sender;
    private readonly ITenantRepository _tenantRepository;
    private readonly IUserRepository _userRepository;
    private readonly IWarehouseRepository _warehouseRepository;

    private static readonly Random _random = new(777);

    public SeedCommandHandler(IProductRepository productRepository, ISender sender, ITenantRepository tenantRepository, IUserRepository userRepository, IWarehouseRepository warehouseRepository)
    {
        _productRepository = productRepository;
        _sender = sender;
        _tenantRepository = tenantRepository;
        _userRepository = userRepository;
        _warehouseRepository = warehouseRepository;
    }

    public async Task Handle(SeedCommand request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await SeedTenantAsync("Global Tenant", "admin@global.com", "GT", "Global Warehouse", cancellationToken);

        await SeedTenantAsync("Antu DevOps", "admin@antudevops.com", "ADO", "Antu DevOps Warehouse", cancellationToken);
    }

    private async Task SeedTenantAsync(
        string tenantName,
        string adminEmail,
        string productCodePrefix,
        string warehouseCode,
        CancellationToken cancellationToken)
    {
        var signUpResult = await CreateUserAsync(tenantName, new Email(adminEmail), cancellationToken);
        
        var productIds = await CreateProductsAsync(signUpResult.TenantId, productCodePrefix, _createdBy, cancellationToken);
        
        var warehouseId = await CreateWarehouseAsync(signUpResult.TenantId, warehouseCode, warehouseCode, cancellationToken);

        await PopulateWarehouseStockAsync(warehouseId, productIds, cancellationToken);

        var productIdsForOrder = productIds.Where(_ => _random.Chance(0.50));

        await CreateOrderAsync(signUpResult.TenantId, warehouseId, productIdsForOrder, cancellationToken);
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

    private async Task<IReadOnlyList<ProductId>> CreateProductsAsync(TenantId tenantId, string codePrefix, string seededBy, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var productCodes = Enumerable
            .Range(1, 100)
            .Select(index => $"{codePrefix}-{index.ToString().PadLeft(3, '0')}");

        var productIds = new List<ProductId>();

        foreach (var code in productCodes)
        {
            var product = await _productRepository.GetByCodeOrDefaultAsync(code, cancellationToken);

            if (product is not null)
            {
                productIds.Add(product.Id);
                continue;
            }

            var productId = await _sender.Send(new CreateProductCommand(
                tenantId,
                code,
                null,
                seededBy));

            productIds.Add(productId);
        }

        return productIds.AsReadOnly();
    }

    private async Task<WarehouseId> CreateWarehouseAsync(TenantId tenantId, string code, string displayName, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var warehouse = await _warehouseRepository.GetByCodeOrDefaultAsync(code, cancellationToken);

        if (warehouse is not null)
            return warehouse.Id;

        return await _sender.Send(new CreateWarehouseCommand(
            tenantId,
            code,
            displayName,
            _createdBy));
    }

    private async Task PopulateWarehouseStockAsync(WarehouseId warehouseId, IEnumerable<ProductId> productIds, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var productId in productIds.ToHashSet())
        {
            var stock = await _sender.Send(new GetStockQuery(warehouseId, productId));

            if (stock is not null)
                continue;

            var quantity = _random.Next(70, 101);

            var price = _random.Next(100, 201);

            await _sender.Send(new AddStockCommand(
                warehouseId,
                productId,
                quantity,
                price),
                cancellationToken);
        }
    }

    private async Task CreateOrderAsync(TenantId tenantId, WarehouseId warehouseId, IEnumerable<ProductId> productIds, CancellationToken cancellationToken)
    {
        foreach (var productId in productIds.ToHashSet())
        {
            try
            {
                var orderId = await _sender.Send(new CreateOrderCommand(tenantId, warehouseId, productId, _createdBy), cancellationToken);

                var increaseQuantity = _random.Next(0, 11);
            
                foreach (var _ in Enumerable.Range(0, increaseQuantity))
                {
                    await _sender.Send(new IncreaseOrderProductQuantityCommand(
                        orderId,
                        warehouseId,
                        productId,
                        _createdBy),
                        cancellationToken);
                }

                var decreaseQuantity = _random.Next(0, 11);

                foreach (var _ in Enumerable.Range(0, decreaseQuantity))
                {
                    await _sender.Send(new DecreaseOrderProductQuantityCommand(
                        orderId,
                        warehouseId,
                        productId,
                        _createdBy),
                        cancellationToken);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
