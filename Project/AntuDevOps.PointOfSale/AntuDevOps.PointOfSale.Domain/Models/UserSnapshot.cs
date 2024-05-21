namespace AntuDevOps.PointOfSale.Domain.Models;

public class UserSnapshot
{
    public UserSnapshot(UserId id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, List<UserTenantRole> roles, Email email, string firstName, string lastName)
    {
        Id = id;
        CreatedAt = createdAt;
        CreatedBy = createdBy;
        LastModifiedAt = lastModifiedAt;
        LastModifiedBy = lastModifiedBy;
        Roles = roles;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public UserId Id { get; }
    public DateTime CreatedAt { get; }
    public string CreatedBy { get; }
    public DateTime? LastModifiedAt { get; }
    public string? LastModifiedBy { get; }
    public List<UserTenantRole> Roles { get; }
    public Email Email { get; }
    public string FirstName { get; }
    public string LastName { get; }
}
