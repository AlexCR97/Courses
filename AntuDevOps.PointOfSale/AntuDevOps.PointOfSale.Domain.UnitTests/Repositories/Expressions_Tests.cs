using AntuDevOps.PointOfSale.Domain.Repositories;
using FluentAssertions;

namespace AntuDevOps.PointOfSale.Domain.UnitTests.Repositories;

public class Expressions_Tests
{
    [Theory]
    [InlineData("1 == 1")]
    [InlineData("tenantId == 1")]
    public void ShouldBuildRawExpression(string inputExpression)
    {
        var builtExpression = new RawExpression(inputExpression).BuildExpression();
        builtExpression.Should().Match(inputExpression);
    }

    [Theory]
    [InlineData("code", "FOO", @"code.Trim().ToLower().Contains(""FOO"".Trim().ToLower())")]
    [InlineData("displayName", "BAR", @"displayName.Trim().ToLower().Contains(""BAR"".Trim().ToLower())")]
    public void ShouldBuildContainsExpression(string property, string value, string expectedExpression)
    {
        var builtExpression = new ContainsExpression(property, value).BuildExpression();
        builtExpression.Should().Match(expectedExpression);
    }

    [Theory]
    [InlineData("code", "FOO", @"code.Trim().ToLower().Contains(""FOO"".Trim().ToLower())")]
    [InlineData("code", null, null)]
    public void ShouldBuildContainsExpressionWithFallback(string property, string? value, string? expectedExpression)
    {
        var containsExpression = ContainsExpression.For(property, value);

        if (string.IsNullOrWhiteSpace(value))
            containsExpression.Should().BeNull();
        else
        {
            containsExpression.Should().NotBeNull();
            var builtExpression = containsExpression!.BuildExpression();
            builtExpression.Should().Match(expectedExpression);
        }
    }

    [Fact]
    public void ShouldBuildAndExpression()
    {
        var builtExpression = new AndExpression()
            .And("1 == 1")
            .And(new RawExpression("tenantId == 1"))
            .And(new ContainsExpression("code", "FOO"))
            .BuildExpression();

        builtExpression.Should().Match($@"(1 == 1) and (tenantId == 1) and (code.Trim().ToLower().Contains(""FOO"".Trim().ToLower()))");
    }

    [Fact]
    public void ShouldBuildOrExpression()
    {
        var builtExpression = new OrExpression()
            .Or("1 == 1")
            .Or(new RawExpression("tenantId == 1"))
            .Or(new ContainsExpression("displayName", "BAR"))
            .BuildExpression();

        builtExpression.Should().Match($@"(1 == 1) or (tenantId == 1) or (displayName.Trim().ToLower().Contains(""BAR"".Trim().ToLower()))");
    }

    [Fact]
    public void ShouldBuildComplexCompundExpression()
    {
        var builtExpression = new AndExpression()
            .And("tenantId == 1")
            .And(new OrExpression()
                .Or($@"1 == 1")
                .Or(new ContainsExpression("code", "FOO"))
                .Or(new AndExpression()
                    .And(new RawExpression("2 == 2"))
                    .And(new ContainsExpression("displayName", "BAR"))))
            .BuildExpression();

        builtExpression.Should().Match($@"(tenantId == 1) and ((1 == 1) or (code.Trim().ToLower().Contains(""FOO"".Trim().ToLower())) or ((2 == 2) and (displayName.Trim().ToLower().Contains(""BAR"".Trim().ToLower()))))");
    }
}
