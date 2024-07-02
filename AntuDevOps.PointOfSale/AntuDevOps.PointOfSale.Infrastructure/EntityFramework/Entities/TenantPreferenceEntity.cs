using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class TenantPreferenceEntity
{
    public TenantPreferenceEntity()
    {
    }

    public TenantPreferenceEntity(int tenantId, TenantEntity? tenant, string key, string? value)
    {
        TenantId = tenantId;
        Tenant = tenant;
        Key = key;
        Value = value;
    }

    public int TenantId { get; set; }
    public TenantEntity? Tenant { get; set; }

    public string Key { get; set; }
    public string? Value { get; set; }

    public void SetValue(string? value)
    {
        Value = value;
    }
}

internal static class TenantPreferenceEntityExtensions
{
    public static TenantPreferenceEntity ToEntity(this TenantPreference a, TenantEntity tenantEntity)
    {
        return new TenantPreferenceEntity(
            tenantEntity.Id,
            null,
            a.Key,
            a.Value);
    }

    public static TenantPreference ToModel(this TenantPreferenceEntity a)
    {
        return new TenantPreference(
            a.Key,
            a.Value);
    }
}
