using AntuDevOps.PointOfSale.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
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

    private static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, int page, int size)
    {
        return queryable
            .Skip((page - 1) * size)
            .Take(size);
    }

    public static async Task<Domain.Repositories.PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> queryable,
        IFindQuery findQuery,
        DbSet<T> dbSet,
        CancellationToken cancellationToken = default)
        where T : class
    {
        cancellationToken.ThrowIfCancellationRequested();

        var results = await queryable.ToListAsync(cancellationToken);

        var totalCount = await dbSet
            // TODO Uncomment this
            //.Search(findQuery.Search)
            .LongCountAsync(cancellationToken);

        var totalPages = (int)(totalCount / findQuery.Size);

        return new Domain.Repositories.PagedResult<T>(
            results,
            new PaginationMetadata(
                CurrentPage: findQuery.Page,
                CurrentSize: findQuery.Size,
                NextPage: findQuery.Page + 1,
                PrevPage: findQuery.Page == 1 ? null : findQuery.Page - 1,
                TotalPages: totalPages,
                TotalCount: totalCount));
    }
}
