namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly record struct TenantId(int Value);

public class Tenant : AggregateRoot<TenantId>
{
    public Tenant(TenantId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, string name) : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        Name = name;
    }

    public string Name { get; private set; }

    public static Tenant SignUp(string name)
    {
        return new Tenant(
            default,
            DateTime.UtcNow,
            "Anonymous",
            null,
            null,
            name);
    }
}
