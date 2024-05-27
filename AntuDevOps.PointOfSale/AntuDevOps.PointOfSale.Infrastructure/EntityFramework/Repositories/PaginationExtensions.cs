using AntuDevOps.PointOfSale.Domain.Repositories;
using System.Linq.Dynamic.Core;

namespace AntuDevOps.PointOfSale.Infrastructure.EntityFramework.Repositories;

internal static class PaginationExtensions
{
    public static IQueryable<T> Find<T>(this IQueryable<T> queryable, IFindQuery findQuery)
    {
        return queryable
            .Sort(findQuery.Sort)
            .Search(findQuery.Search)
            .Paginate(findQuery.Page, findQuery.Size);
    }

    private static IQueryable<T> Sort<T>(this IQueryable<T> queryable, Sort? sort)
    {
        return sort is null
            ? queryable
            : queryable.OrderBy(sort.Value.ToString());
    }

    private static IQueryable<T> Search<T>(this IQueryable<T> queryable, string? search)
    {
        return string.IsNullOrWhiteSpace(search)
            ? queryable
            : queryable.Where(search);
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int page, int size)
    {
        return queryable
            .Skip((page - 1) * size)
            .Take(size);
    }
}
