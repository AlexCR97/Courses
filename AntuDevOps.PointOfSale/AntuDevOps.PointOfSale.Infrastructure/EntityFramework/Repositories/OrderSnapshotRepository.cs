using AntuDevOps.PointOfSale.Domain.Models;
using AntuDevOps.PointOfSale.Domain.Repositories;
using AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;

internal class OrderSnapshotRepository : IOrderSnapshotRepository
{
    private readonly PointOfSaleDbContext _dbContext;

    public OrderSnapshotRepository(PointOfSaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<OrderSnapshotId> CreateAsync(OrderSnapshot orderSnapshotModel, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var orderSnapshotEntity = orderSnapshotModel.ToEntity();

        await _dbContext.OrderSnapshots.AddAsync(orderSnapshotEntity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new OrderSnapshotId(orderSnapshotEntity.Id);
    }

    public Task DeleteAsync(OrderSnapshotId id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<PagedResult<OrderSnapshot>> FindAsync(IFindQuery query, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var pagedResult = await _dbContext.OrderSnapshots
            .AsNoTracking()
            .Find(query)
            .ToPagedResultAsync(query, _dbContext.OrderSnapshots, cancellationToken);

        return pagedResult.Map(x => x.ToModel());
    }

    public async Task<OrderSnapshot> GetAsync(OrderSnapshotId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return await GetOrDefaultAsync(id, cancellationToken)
            ?? throw new InvalidOperationException($"No such OrderSnapshot with Id={id}");
    }

    public async Task<OrderSnapshot?> GetOrDefaultAsync(OrderSnapshotId id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var entity = await _dbContext.OrderSnapshots
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id.Value, cancellationToken);

        if (entity is null)
            return null;

        return entity.ToModel();
    }

    public Task UpdateAsync(OrderSnapshot orderSnapshotModel, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
