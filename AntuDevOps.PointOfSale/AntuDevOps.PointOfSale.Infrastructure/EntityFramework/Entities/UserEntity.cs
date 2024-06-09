using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;

internal class UserEntity : BaseEntity
{
    public UserEntity()
    {
    }

    public UserEntity(int id, DateTime createdAt, string createdBy, DateTime? lastModifiedAt, string? lastModifiedBy, string email, string emailNormalized, string password, string firstName, string lastName) : base(id, createdAt, createdBy, lastModifiedAt, lastModifiedBy)
    {
        Email = email;
        EmailNormalized = emailNormalized;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
    }

    public string Email { get; set; }
    public string EmailNormalized { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

internal static class UserEntityExtensions
{
    public static UserEntity ToEntity(this User user)
    {
        return new UserEntity(
            user.Id.Value,
            user.CreatedAt,
            user.CreatedBy,
            user.LastModifiedAt,
            user.LastModifiedBy,
            user.Email.OriginalValue,
            user.Email.NormalizedValue,
            user.Password.Value,
            user.FirstName,
            user.LastName);
    }

    public static User ToModel(this UserEntity user, IReadOnlyList<TenantUserEntity> tenantUserEntities)
    {
        return new User(
            new UserId(user.Id),
            user.CreatedAt,
            user.CreatedBy,
            user.LastModifiedAt,
            user.LastModifiedBy,
            tenantUserEntities
                .Select(x => new UserTenant(
                    new TenantId(x.TenantId),
                    Enum.Parse<UserTenantRole>(x.Role)))
                .ToList(),
            new Email(user.Email),
            new Password(user.Password),
            user.FirstName,
            user.LastName);
    }
}
