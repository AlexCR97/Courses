namespace AntuDevOps.PointOfSale.Domain.Repositories;

public interface IExpression
{
    string BuildExpression();
}

public class RawExpression : IExpression
{
    public RawExpression(string expression)
    {
        Expression = expression;
    }

    private string Expression { get; }

    public string BuildExpression()
    {
        return string.IsNullOrWhiteSpace(Expression)
            ? "true"
            : $"{Expression}";
    }
}

public class ContainsExpression : IExpression
{
    public ContainsExpression(string property, string value)
    {
        Property = property;
        Value = value;
    }

    private string Property { get; }
    private string Value { get; }

    public string BuildExpression()
    {
        return @$"{Property}.Trim().ToLower().Contains(""{Value}"".Trim().ToLower())";
    }

    public static ContainsExpression? For(string property, string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : new ContainsExpression(property, value);
    }
}

public class AndExpression : CompoundExpression
{
    public AndExpression() : base() { }
    public AndExpression(string? expression = null) : base(expression) { }
    public AndExpression(IExpression? expression = null) : base(expression) { }

    public AndExpression And(string? expression)
    {
        TryAddExpression(expression);
        return this;
    }

    public AndExpression And(IExpression? expression)
    {
        TryAddExpression(expression);
        return this;
    }

    public override string BuildExpression()
    {
        if (Expressions.Count == 0)
            return "true";

        var builtExpressions = Expressions.Select(x => $"({x.BuildExpression()})");
        return string.Join(" and ", builtExpressions);
    }
}

public class OrExpression : CompoundExpression
{
    public OrExpression() { }
    public OrExpression(string? expression = null) : base(expression) { }
    public OrExpression(IExpression? expression = null) : base(expression) { }

    public OrExpression Or(string? expression)
    {
        TryAddExpression(expression);
        return this;
    }

    public OrExpression Or(IExpression? expression)
    {
        TryAddExpression(expression);
        return this;
    }

    public override string BuildExpression()
    {
        if (Expressions.Count == 0)
            return "true";

        var builtExpressions = Expressions.Select(x => $"({x.BuildExpression()})");
        return string.Join(" or ", builtExpressions);
    }
}

public abstract class CompoundExpression : IExpression
{
    public CompoundExpression() { }
    public CompoundExpression(string? expression) => TryAddExpression(expression);
    public CompoundExpression(IExpression? expression) => TryAddExpression(expression);

    protected List<IExpression> Expressions { get; } = new();

    public abstract string BuildExpression();

    protected void TryAddExpression(string? expression)
    {
        if (!string.IsNullOrWhiteSpace(expression))
            TryAddExpression(new RawExpression(expression));
    }

    protected void TryAddExpression(IExpression? expression)
    {
        if (expression is not null)
            Expressions.Add(expression);
    }
}
