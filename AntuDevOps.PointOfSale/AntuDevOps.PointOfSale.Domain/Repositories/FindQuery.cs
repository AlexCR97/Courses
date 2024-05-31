using FluentValidation;

namespace AntuDevOps.PointOfSale.Domain.Repositories;

public interface IFindQuery
{
    public int Page { get; }
    public int Size { get; }
    public Sort? Sort { get; }
}

public record FindQuery : IFindQuery
{
    public const int PageMin = 1;
    public const int PageDefault = 1;

    public const int SizeMin = 1;
    public const int SizeMax = 100;
    public const int SizeDefault = 10;

    public FindQuery(
        int page,
        int size,
        Sort? sort)
    {
        Page = page;
        Size = size;
        Sort = sort;
        new Validator().ValidateAndThrow(this);
    }

    public int Page { get; init; }
    public int Size { get; init; }
    public Sort? Sort { get; init; }

    public static FindQuery Create(
        int page = PageDefault,
        int size = SizeDefault,
        Sort? sort = null)
    {
        return new FindQuery(page, size, sort);
    }

    private class Validator : AbstractValidator<FindQuery>
    {
        public Validator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(PageMin);

            RuleFor(x => x.Size)
                .GreaterThanOrEqualTo(SizeMin)
                .LessThanOrEqualTo(SizeMax);
        }
    }
}

public readonly struct Sort
{
    public Sort()
    {
        Value = string.Empty;
        new Validator().ValidateAndThrow(this);
    }

    public Sort(string value)
    {
        Value = value;
        new Validator().ValidateAndThrow(this);
    }

    public static Sort? ParseOrDefault(string? sort)
    {
        return string.IsNullOrWhiteSpace(sort)
            ? null
            : new Sort(sort);
    }

    private string Value { get; }

    private string[] SortParts => Value.Split(' ');
    
    public string SortBy => SortParts[0];

    public SortOrder SortOrder => SortParts
        .ElementAtOrDefault(1)
        ?.ToSortOrder()
        ?? SortOrder.Ascending;

    public override string ToString()
    {
        return $"{SortBy} {SortOrder.ToDisplayString()}";
    }

    private class Validator : AbstractValidator<Sort>
    {
        public Validator()
        {
            RuleFor(x => x.Value)
                .NotEmpty();

            RuleFor(x => x.SortParts)
                .Must(sortParts =>
                {
                    if (sortParts.Length == 1)
                    {
                        var sortBy = sortParts[0];
                        return !string.IsNullOrWhiteSpace(sortBy);
                    }

                    if (sortParts.Length == 2)
                    {
                        var sortBy = sortParts[0];
                        var sortOrder = sortParts[1];
                        return !string.IsNullOrWhiteSpace(sortBy) && sortOrder.ToSortOrderOrDefault() is not null;
                    }

                    return false;
                })
                .WithMessage(@$"Sort format should be any of: ""<sortBy>"", ""<sortBy> asc"", ""<sortBy> desc""");
        }
    }
}

public enum SortOrder
{
    Ascending,
    Descending,
}

public static class SortOrderExtensions
{
    private const string _asc = "asc";
    private const string _desc = "desc";

    public static SortOrder? ToSortOrder(this string str)
    {
        return str.ToSortOrderOrDefault()
            ?? throw new InvalidOperationException(@$"Invalid sort order: ""{str}""");
    }

    public static SortOrder? ToSortOrderOrDefault(this string str)
    {
        if (string.Equals(str, _asc, StringComparison.OrdinalIgnoreCase))
            return SortOrder.Ascending;

        if (string.Equals(str, _desc, StringComparison.OrdinalIgnoreCase))
            return SortOrder.Descending;

        return null;
    }

    public static string ToDisplayString(this SortOrder order)
    {
        return order switch
        {
            SortOrder.Ascending => _asc,
            SortOrder.Descending => _desc,
            _ => throw new NotSupportedException(),
        };
    }
}
