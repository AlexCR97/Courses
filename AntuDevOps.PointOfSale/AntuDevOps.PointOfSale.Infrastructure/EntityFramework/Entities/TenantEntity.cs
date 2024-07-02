using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class TenantEntity : BaseEntity
{
    public TenantEntity()
    {
    }

    public TenantEntity(int id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, string name) : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        Name = name;
    }

    public string Name { get; set; }
}

internal static class TenantEntityExtensions
{
    public static TenantEntity ToEntity(this Tenant a)
    {
        return new TenantEntity(
            a.Id.Value,
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.Name);
    }

    public static Tenant ToModel(this TenantEntity a, IReadOnlyList<TenantPreference> preferences)
    {
        return new Tenant(
            new TenantId(a.Id),
            a.CreatedAt,
            a.CreatedBy,
            a.LastModifiedAt,
            a.LastModifiedBy,
            a.Name,
            preferences);
    }
}
