using AntuDevOps.PointOfSale.Domain.Repositories;
using FluentAssertions;
using FluentValidation;

namespace AntuDevOps.PointOfSale.Domain.UnitTests.Repositories;

public class FindQuery_Sort_Tests
{
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void ShouldHandleNullSort(string? value)
    {
        var sort = Sort.ParseOrDefault(value);
        sort.Should().BeNull();
    }

    [Theory]
    [InlineData("displayName", "displayName", SortOrder.Ascending)]
    [InlineData("displayName asc", "displayName", SortOrder.Ascending)]
    [InlineData("displayName desc", "displayName", SortOrder.Descending)]
    public void ShouldCreateValidSort(string value, string expectedSortBy, SortOrder expectedSortOrder)
    {
        var sort = new Sort(value);
        sort.SortBy.Should().Match(expectedSortBy);
        sort.SortOrder.Should().Be(expectedSortOrder);
    }

    [Theory]
    [InlineData("displayName   ")]
    [InlineData("displayName foo")]
    [InlineData("displayName foo bar")]
    public void ShouldThrowForInvalidSort(string value)
    {
        FluentActions
            .Invoking(() =>
            {
                var sort = new Sort(value);
            })
            .Should()
            .ThrowExactly<ValidationException>();
    }
}
