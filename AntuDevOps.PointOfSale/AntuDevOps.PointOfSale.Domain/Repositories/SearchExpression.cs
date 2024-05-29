namespace AntuDevOps.PointOfSale.Domain.Repositories;

public interface IExpressionBuilder
{
    string BuildExpression();
}

public abstract class ExpressionBuilder : IExpressionBuilder
{
    public ExpressionBuilder(string? expression = null)
    {
        TryAddExpression(expression);
    }

    protected List<string> Expressions { get; } = new();

    public abstract string BuildExpression();

    protected void TryAddExpression(string? expression)
    {
        if (!string.IsNullOrWhiteSpace(expression))
            Expressions.Add($"({expression})");
    }
}

public class AndExpression : ExpressionBuilder
{
    public AndExpression(string? expression = null) : base(expression) { }

    public AndExpression And(string? expression)
    {
        TryAddExpression(expression);
        return this;
    }

    public override string BuildExpression()
    {
        return string.Join(" and ", Expressions);
    }
}

public class OrExpression : ExpressionBuilder
{
    public OrExpression(string? expression = null) : base(expression) { }

    public OrExpression Or(string? expression)
    {
        TryAddExpression(expression);
        return this;
    }

    public override string BuildExpression()
    {
        return string.Join(" or ", Expressions);
    }
}
