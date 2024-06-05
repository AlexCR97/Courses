using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Domain.Repositories;

public interface IWarehouseRepository : IRepository<Warehouse, WarehouseId>
{
    Task<Warehouse?> GetByCodeOrDefaultAsync(string code, CancellationToken cancellationToken = default);
}
