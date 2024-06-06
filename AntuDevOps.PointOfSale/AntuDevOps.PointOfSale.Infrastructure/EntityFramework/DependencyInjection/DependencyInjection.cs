using AntuDevOps.PointOfSale.Domain.Repositories;
using AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.DependencyInjection;

public static class DependencyInjection
{
    internal static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<PointOfSaleDbContext>(options =>
            {
                var connectionString = configuration.GetRequiredSection("ConnectionStrings:Localhost").Value;

                options
                    .UseSqlServer(connectionString)
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }, ServiceLifetime.Transient)
            .AddTransient<IOrderRepository, OrderRepository>()
            .AddTransient<IOrderSnapshotRepository, OrderSnapshotRepository>()
            .AddTransient<IProductRepository, ProductRepository>()
            .AddTransient<ITenantRepository, TenantRepository>()
            .AddTransient<IUserRepository, UserRepository>()
            .AddTransient<IWarehouseRepository, WarehouseRepository>();
    }
}
