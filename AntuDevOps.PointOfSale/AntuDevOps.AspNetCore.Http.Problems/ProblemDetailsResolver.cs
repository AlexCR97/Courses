using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AntuDevOps.AspNetCore.Http.Problems;

public interface IProblemDetailsResolver
{
    ProblemDetails Resolve<TException>(TException exception)
        where TException : Exception;
}

internal record ProblemDetailsResolverOptions(
    IConfiguration? ProblemDetailsConfiguration);

internal class ProblemDetailsResolver : IProblemDetailsResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;
    private readonly ProblemDetailsResolverOptions _options;

    public ProblemDetailsResolver(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider, ProblemDetailsResolverOptions options)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
        _options = options;
    }

    public ProblemDetails Resolve<TException>(TException exception)
        where TException : Exception
    {
        var builder = new ProblemDetailsBuilder();

        builder = BuildProblemDetails(builder, exception, typeof(Exception));

        if (exception.GetType() != typeof(Exception))
            builder = BuildProblemDetails(builder, exception, exception.GetType());

        return builder.Build();
    }

    private ProblemDetailsBuilder BuildProblemDetails(ProblemDetailsBuilder builder, Exception exception, Type exceptionType)
    {
        var (factory, specificExceptionType) = GetFactoryAndExceptionType(exceptionType);

        var contextType = typeof(ProblemDetailsContext<>).MakeGenericType(specificExceptionType);

        var context = Activator.CreateInstance(
            contextType,
            _options.ProblemDetailsConfiguration,
            _httpContextAccessor.HttpContext,
            exception,
            builder);

        var problemDetails = CreateProblemDetails(factory, context!);

        return ProblemDetailsBuilder.From(problemDetails);
    }

    private (object Factory, Type Type) GetFactoryAndExceptionType(Type? exceptionType)
    {
        ArgumentNullException.ThrowIfNull(exceptionType);

        var factoryType = typeof(IProblemDetailsFactory<>).MakeGenericType(exceptionType);
        var factory = _serviceProvider.GetService(factoryType);

        if (factory is not null)
            return (factory, exceptionType);

        return GetFactoryAndExceptionType(exceptionType.BaseType);
    }

    private static ProblemDetails CreateProblemDetails(object problemDetailsFactory, object problemDetailsContext)
    {
        var problemDetailsObj = problemDetailsFactory
            ?.GetType()
            ?.GetMethod(nameof(IProblemDetailsFactory<Exception>.CreateProblemDetails))
            ?.Invoke(problemDetailsFactory, new[] { problemDetailsContext });

        if (problemDetailsObj is ProblemDetails problemDetails)
            return problemDetails;

        throw new InvalidOperationException(@$"Could not create {nameof(ProblemDetails)} using factory ""{problemDetailsFactory!.GetType().Name}"" and context ""{problemDetailsContext.GetType().Name}"".");
    }
}
