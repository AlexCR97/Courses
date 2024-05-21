using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Domain.Repositories;

public interface ITenantRepository : IRepository<Tenant, TenantId>
{
    Task<Tenant?> GetByNameOrDefaultAsync(string name, CancellationToken cancellationToken = default);
}
