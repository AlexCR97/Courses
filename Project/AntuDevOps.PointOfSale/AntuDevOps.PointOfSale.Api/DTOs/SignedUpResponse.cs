using AntuDevOps.PointOfSale.Application.Users;

namespace AntuDevOps.PointOfSale.Api.DTOs;

public record SignedUpResponse(
    int TenantId,
    int UserId);

internal static class SignedUpResponseExtensions
{
    public static SignedUpResponse ToResponse(this UserSignedUpResult result)
    {
        return new SignedUpResponse(
            result.TenantId.Value,
            result.UserId.Value);
    }
}
