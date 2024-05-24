using System.Text.Json.Serialization;

namespace AntuDevOps.PointOfSale.Api.OAuth;

public record OAuthTokenResponse(
    [property:JsonPropertyName("access_token")]
    string AccessToken,

    [property:JsonPropertyName("token_type")]
    [property:JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? TokenType,

    [property: JsonPropertyName("expires_in")]
    [property:JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    int? ExpiresIn,

    [property: JsonPropertyName("scope")]
    [property:JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? Scope,

    [property: JsonPropertyName("id_token")]
    [property:JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? IdToken,

    [property: JsonPropertyName("refresh_token")]
    [property:JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? RefreshToken);
