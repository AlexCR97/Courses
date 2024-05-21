using AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework;

internal class PointOfSaleDbContext : DbContext
{
    public PointOfSaleDbContext(DbContextOptions<PointOfSaleDbContext> options)
        : base(options)
    {
    }

    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<TenantEntity> Tenants { get; set; }
    public DbSet<TenantUserEntity> TenantUsers { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<ProductEntity>()
            .HasIndex(x => x.Code)
            .IsUnique();

        modelBuilder
            .Entity<TenantEntity>()
            .HasIndex(x => x.Name)
            .IsUnique();

        modelBuilder
            .Entity<TenantUserEntity>()
            .HasKey(x => new { x.TenantId, x.UserId });

        modelBuilder
            .Entity<UserEntity>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder
            .Entity<UserEntity>()
            .HasIndex(x => x.EmailNormalized)
            .IsUnique();
    }
}
