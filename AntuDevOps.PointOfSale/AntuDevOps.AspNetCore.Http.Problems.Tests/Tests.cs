using AntuDevOps.AspNetCore.Http.Problems.DependencyInjection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace AntuDevOps.AspNetCore.Http.Problems.Tests;

public class Tests
{
    [Fact]
    public void Test()
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ProblemDetails:IncludeException"] = "true",
            })
            .Build();

        var services = new ServiceCollection()
            .AddSingleton<IHttpContextAccessor, MockHttpContextAccessor>()
            .AddProblemDetails(configuration)
            .AddProblemDetailsFactory<CustomParentException, CustomParentExceptionProblemDetailsFactory>()
            .AddProblemDetailsFactory<CustomChildFooException, CustomChildFooExceptionProblemDetailsFactory>()
            .BuildServiceProvider();

        var problemDetailsResolver = services.GetRequiredService<IProblemDetailsResolver>();

        var exceptionProblemDetails = problemDetailsResolver.Resolve(new Exception("This is a test Exception!"));

        var parentExceptionProblemDetails = problemDetailsResolver.Resolve(new CustomParentException());

        var childFooExceptionProblemDetails = problemDetailsResolver.Resolve(new CustomChildFooException());

        var childBarExceptionProblemDetails = problemDetailsResolver.Resolve(new CustomChildBarException());
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
