using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Domain.Repositories;

public interface IUserRepository : IRepository<User, UserId>
{
    Task<User?> GetByEmailOrDefaultAsync(Email email, CancellationToken cancellationToken = default);
}
