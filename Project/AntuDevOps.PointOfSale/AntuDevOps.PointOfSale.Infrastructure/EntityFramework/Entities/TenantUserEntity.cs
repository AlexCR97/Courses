using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class TenantUserEntity
{
    public TenantUserEntity()
    {
    }

    public TenantUserEntity(int tenantId, TenantEntity? tenant, int userId, UserEntity? user, string role)
    {
        TenantId = tenantId;
        Tenant = tenant;
        UserId = userId;
        User = user;
        Role = role;
    }

    public int TenantId { get; set; }
    public TenantEntity? Tenant { get; set; }

    public int UserId { get; set; }
    public UserEntity? User { get; set; }

    public string Role { get; set; }
}
