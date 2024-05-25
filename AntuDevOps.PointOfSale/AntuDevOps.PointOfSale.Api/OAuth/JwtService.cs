using System.Security.Claims;

namespace AntuDevOps.PointOfSale.Api.OAuth;

public class JwtService
{
    private readonly JwtOptions _jwtOptions;

    public JwtService(JwtOptions jwtOptions)
    {
        _jwtOptions = jwtOptions;
    }

    public string GenerateJwt(IReadOnlyList<Claim> claims)
    {
        return JwtGenerator.Generate(
            _jwtOptions.SecretKey,
            issuer: string.IsNullOrWhiteSpace(_jwtOptions.Issuer)
                ? null
                : _jwtOptions.Issuer,
            audience: string.IsNullOrWhiteSpace(_jwtOptions.Audience)
                ? null
                : _jwtOptions.Audience,
            expiresInMinutes: _jwtOptions.ExpiresInMinutes,
            claims: claims);
    }
}
