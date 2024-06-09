using Microsoft.Extensions.DependencyInjection;

namespace AntuDevOps.PointOfSale.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services
            .AddMediatR(config => config
                .RegisterServicesFromAssemblyContaining(typeof(DependencyInjection)));
    }
}
