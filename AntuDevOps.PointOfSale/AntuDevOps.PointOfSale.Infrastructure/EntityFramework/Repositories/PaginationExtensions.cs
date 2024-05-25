using AntuDevOps.PointOfSale.Domain.Repositories;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;

internal static class PaginationExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, IFindQuery findQuery)
    {
        return queryable
            .Skip((findQuery.Page - 1) * findQuery.Size)
            .Take(findQuery.Size);
    }
}
