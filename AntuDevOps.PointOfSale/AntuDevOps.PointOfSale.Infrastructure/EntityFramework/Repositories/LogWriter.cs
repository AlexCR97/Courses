using AntuDevOps.PointOfSale.Application.Logging;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;

internal class LogWriter : ILogWriter
{
    private readonly PointOfSaleDbContext _dbContext;

    public LogWriter(PointOfSaleDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task WriteAsync(Log log)
    {
        await _dbContext.Logs.AddAsync(new Entities.LogEntity(
            default,
            log.Timestamp,
            log.Level,
            log.MessageTemplate,
            log.Properties));

        await _dbContext.SaveChangesAsync();
    }
}
