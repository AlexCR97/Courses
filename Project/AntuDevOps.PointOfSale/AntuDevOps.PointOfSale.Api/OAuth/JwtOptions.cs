namespace AntuDevOps.PointOfSale.Api.OAuth;

public record JwtOptions(
    string SecretKey,
    string? Issuer,
    string? Audience,
    int? ExpiresInMinutes);
