namespace AntuDevOps.PointOfSale.Domain.Repositories;

public record PagedResult<T>(
    IReadOnlyList<T> Results,
    PaginationMetadata Pagination)
{
    public PagedResult<U> Map<U>(Func<T, U> selector)
    {
        var mapped = Results
            .Select(selector)
            .ToList();

        return new PagedResult<U>(
            mapped,
            new PaginationMetadata(
                Pagination.CurrentPage,
                Pagination.CurrentSize,
                Pagination.NextPage,
                Pagination.PrevPage,
                Pagination.TotalPages,
                Pagination.TotalCount));
    }

    public PagedResult<U> WithResults<U>(IEnumerable<U> results)
    {
        return new PagedResult<U>(
            results.ToList(),
            new PaginationMetadata(
                Pagination.CurrentPage,
                Pagination.CurrentSize,
                Pagination.NextPage,
                Pagination.PrevPage,
                Pagination.TotalPages,
                Pagination.TotalCount));
    }
}

public record PaginationMetadata(
    int CurrentPage,
    int CurrentSize,
    int NextPage,
    int? PrevPage,
    int TotalPages,
    long TotalCount);
