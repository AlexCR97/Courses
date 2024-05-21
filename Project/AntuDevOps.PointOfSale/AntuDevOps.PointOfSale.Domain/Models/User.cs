namespace AntuDevOps.PointOfSale.Domain.Models;

public readonly record struct UserId(int Value);

public class User : AggregateRoot<UserId>
{
    public User(UserId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, IReadOnlyList<UserTenant> tenants, Email email, Password password, string firstName, string lastName) : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        _tenants = tenants.ToList();
        Email = email;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
    }

    public IReadOnlyList<UserTenant> Tenants => _tenants;
    private readonly List<UserTenant> _tenants;

    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public static User SignUp(Tenant tenant, UserTenantRole role, Email email, Password password, string firstName, string lastName)
    {
        return new User(
            default,
            DateTime.UtcNow,
            "Anonymous",
            null,
            null,
            new List<UserTenant>
            {
                new UserTenant(tenant.Id, role),
            },
            email,
            password,
            firstName,
            lastName);
    }
}

public class UserTenant
{
    public UserTenant(TenantId tenantId, UserTenantRole role)
    {
        TenantId = tenantId;
        Role = role;
    }

    public TenantId TenantId { get; }
    public UserTenantRole Role { get; private set; }
}

public enum UserTenantRole
{
    Owner,
}
