using FluentAssertions;
using FluentResults;

namespace AntuDevOps.PointOfSale.Domain.UnitTests;

public class FluentResults_Tests
{
    [Fact]
    public void ShouldDivideByOne()
    {
        var result = Divide(10, 1);
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotDivideByZero()
    {
        var result = Divide(10, 0);
        result.IsFailed.Should().BeTrue();
    }

    Result<double> Divide(double a, double b)
    {
        if (b == 0)
            return Result.Fail("Cannot divide by zero");

        return a / b;
    }
}
