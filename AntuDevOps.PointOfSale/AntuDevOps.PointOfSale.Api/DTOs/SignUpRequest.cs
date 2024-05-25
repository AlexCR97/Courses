using AntuDevOps.PointOfSale.Application.Users;
using AntuDevOps.PointOfSale.Domain.Models;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record SignUpRequest(
    string TenantName,
    string Role,
    string Email,
    string Password,
    string FirstName,
    string LastName)
{
    internal SignUpCommand ToCommand()
    {
        return new SignUpCommand(
            TenantName,
            Enum.Parse<UserTenantRole>(Role),
            new Email(Email),
            new Password(Password),
            FirstName,
            LastName);
    }
}
