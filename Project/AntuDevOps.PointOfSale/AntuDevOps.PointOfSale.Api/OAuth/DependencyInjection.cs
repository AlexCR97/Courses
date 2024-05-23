namespace AntuDevOps.PointOfSale.Api.OAuth;

public static class DependencyInjection
{
    public static IServiceCollection AddJwtService(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddSingleton<JwtOptions>((_) =>
            {
                var secretKey = configuration.GetRequiredSection("Jwt:SecretKey").Value!;
                var issuer = configuration.GetRequiredSection("Jwt:Issuer").Value!;
                var audience = configuration.GetRequiredSection("Jwt:Audience").Value!;
                var expiresInMinutes = int.Parse(configuration.GetRequiredSection("Jwt:ExpiresInMinutes").Value!);

                return new JwtOptions(
                    secretKey,
                    issuer,
                    audience,
                    expiresInMinutes);
            })
            .AddScoped<JwtService>();
    }
}
