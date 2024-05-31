//using AntuDevOps.PointOfSale.Domain.Repositories;
//using FluentAssertions;
//using FluentValidation;

//namespace AntuDevOps.PointOfSale.Domain.UnitTests.Repositories;

//public class FindQuery_Tests
//{
//    [Theory]
//    [InlineData(FindQuery.PageMin, FindQuery.SizeMin)]
//    [InlineData(FindQuery.PageMin, FindQuery.SizeMax)]
//    [InlineData(FindQuery.PageMin, FindQuery.SizeDefault)]
//    [InlineData(FindQuery.PageDefault, FindQuery.SizeMin)]
//    [InlineData(FindQuery.PageDefault, FindQuery.SizeMax)]
//    [InlineData(FindQuery.PageDefault, FindQuery.SizeDefault)]
//    public void ShouldCreateValidFindQuery(int page, int size)
//    {
//        var query = FindQuery.Create(page, size);
//        query.Should().NotBeNull();
//    }

//    [Theory]
//    [InlineData(FindQuery.PageMin - 1, FindQuery.SizeDefault)]
//    [InlineData(FindQuery.PageDefault, FindQuery.SizeMin - 1)]
//    [InlineData(FindQuery.PageDefault, FindQuery.SizeMax + 1)]
//    public void ShouldThrowForInvalidFindQuery(int page, int size)
//    {
//        FluentActions
//            .Invoking(() =>
//            {
//                var query = FindQuery.Create(page, size);
//            })
//            .Should()
//            .ThrowExactly<ValidationException>();
//    }
//}
