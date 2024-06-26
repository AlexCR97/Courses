using AntuDevOps.AspNetCore.Http.Problems.DependencyInjection;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AntuDevOps.AspNetCore.Http.Problems.Tests;

public class Tests
{
    public Tests()
    {
        Configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ProblemDetails:IncludeException"] = "true",
            })
            .Build();
    }

    private IConfiguration Configuration { get; }

    [Fact]
    public void ShouldAddResolver()
    {
        var services = new ServiceCollection()
            .AddSingleton<IHttpContextAccessor, MockHttpContextAccessor>()
            .AddProblemDetails(_ => { })
            .BuildServiceProvider();

        var resolver = services.GetService<IProblemDetailsResolver>();
        resolver.Should().NotBeNull();
        resolver.Should().BeAssignableTo<IProblemDetailsResolver>();
    }

    [Fact]
    public void ShouldAddDefaultFactory()
    {
        var services = new ServiceCollection()
            .AddSingleton<IHttpContextAccessor, MockHttpContextAccessor>()
            .AddProblemDetails(_ => { })
            .BuildServiceProvider();

        var factory = services.GetService<IProblemDetailsFactory<Exception>>();
        factory.Should().NotBeNull();
        factory.Should().BeAssignableTo<IProblemDetailsFactory<Exception>>();
    }

    [Fact]
    public void ShouldHandleExceptions()
    {
        var services = new ServiceCollection()
            .AddSingleton<IHttpContextAccessor, MockHttpContextAccessor>()
            .AddProblemDetails(builder => builder
                .WithConfiguration(Configuration)
                .AddProblemDetailsFactory<CustomParentException, CustomParentExceptionProblemDetailsFactory>()
                .AddProblemDetailsFactory<CustomChildFooException, CustomChildFooExceptionProblemDetailsFactory>())
            .BuildServiceProvider();

        var resolver = services.GetRequiredService<IProblemDetailsResolver>();

        var exceptionProblemDetails = resolver.Resolve(new Exception("This is a test Exception!"));
        exceptionProblemDetails.Title.Should().Match("Unknown");

        var parentExceptionProblemDetails = resolver.Resolve(new CustomParentException());
        parentExceptionProblemDetails.Title.Should().Match("Parent!");

        var childFooExceptionProblemDetails = resolver.Resolve(new CustomChildFooException());
        childFooExceptionProblemDetails.Title.Should().Match("Foo!");

        var childBarExceptionProblemDetails = resolver.Resolve(new CustomChildBarException());
        childBarExceptionProblemDetails.Title.Should().Match("Parent!");
    }

    private class MockHttpContextAccessor : IHttpContextAccessor
    {
        public HttpContext HttpContext { get; set; } = new MockHttpContext();
    }

    private class MockHttpContext : HttpContext
    {
        public override IFeatureCollection Features => throw new NotImplementedException();
        public override HttpRequest Request => new MockHttpRequest();
        public override HttpResponse Response => throw new NotImplementedException();
        public override ConnectionInfo Connection => throw new NotImplementedException();
        public override WebSocketManager WebSockets => throw new NotImplementedException();

        [Obsolete]
        public override AuthenticationManager Authentication => throw new NotImplementedException();

        public override ClaimsPrincipal User { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IServiceProvider RequestServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override CancellationToken RequestAborted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override void Abort() => throw new NotImplementedException();
    }

    private class MockHttpRequest : HttpRequest
    {
        public override HttpContext HttpContext => throw new NotImplementedException();
        public override string Method { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Scheme { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsHttps { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override HostString Host { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override PathString PathBase { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override PathString Path { get; set; } = string.Empty;
        public override QueryString QueryString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IQueryCollection Query { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string Protocol { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override IHeaderDictionary Headers => throw new NotImplementedException();
        public override IRequestCookieCollection Cookies { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override long? ContentLength { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string ContentType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Stream Body { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool HasFormContentType => throw new NotImplementedException();
        public override IFormCollection Form { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default) => throw new NotImplementedException();
    }

    private class CustomParentException : Exception;

    private class CustomChildFooException : CustomParentException;

    private class CustomChildBarException : CustomParentException;

    private class CustomParentExceptionProblemDetailsFactory : IProblemDetailsFactory<CustomParentException>
    {
        public ProblemDetails CreateProblemDetails(ProblemDetailsContext<CustomParentException> context)
        {
            return context.Problem
                .WithTitle("Parent!")
                .Build();
        }
    }

    private class CustomChildFooExceptionProblemDetailsFactory : IProblemDetailsFactory<CustomChildFooException>
    {
        public ProblemDetails CreateProblemDetails(ProblemDetailsContext<CustomChildFooException> context)
        {
            return context.Problem
                .WithTitle("Foo!")
                .Build();
        }
    }
}
