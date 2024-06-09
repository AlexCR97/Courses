using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Domain.Repositories;

public interface IProductRepository : IRepository<Product, ProductId>
{
    Task<Product?> GetByCodeOrDefaultAsync(string code, CancellationToken cancellationToken);
}
