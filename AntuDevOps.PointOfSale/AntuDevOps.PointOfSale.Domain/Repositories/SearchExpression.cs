namespace AntuDevOps.PointOfSale.Domain.Repositories;

public class SearchExpression
{
    private readonly List<string> _expressions = new();

    public SearchExpression(string? expression = null)
    {
        TryAddExpression(expression);
    }

    public SearchExpression And(string? expression)
    {
        TryAddExpression(expression);
        return this;
    }

    public void TryAddExpression(string? expression)
    {
        if (!string.IsNullOrWhiteSpace(expression))
            _expressions.Add($"({expression})");
    }

    public string Build()
    {
        return string.Join(" and ", _expressions);
    }
}
