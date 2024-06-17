using Microsoft.AspNetCore.Http;

namespace AntuDevOps.AspNetCore.Http.Problems;

public interface IProblemDetailsResolver
{
    ProblemDetails Resolve<TException>(TException exception)
        where TException : Exception;
}

public class ProblemDetailsResolver : IProblemDetailsResolver
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IServiceProvider _serviceProvider;
    private readonly ProblemDetailsFactoryOptions _problemDetailsFactoryOptions;

    public ProblemDetailsResolver(IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider, ProblemDetailsFactoryOptions problemDetailsFactoryOptions)
    {
        _httpContextAccessor = httpContextAccessor;
        _serviceProvider = serviceProvider;
        _problemDetailsFactoryOptions = problemDetailsFactoryOptions;
    }

    public ProblemDetails Resolve<TException>(TException exception)
        where TException : Exception
    {
        var builder = new ProblemDetailsBuilder();

        var baseContext = new ProblemDetailsContext<Exception>(
            _problemDetailsFactoryOptions,
            _httpContextAccessor.HttpContext,
            exception,
            builder);

        var (defaultFactory, _) = GetFactory(typeof(Exception));
        GetProblemDetails(defaultFactory, baseContext);

        if (exception.GetType() == typeof(Exception))
        {
            // No need to intercept exception further
        }
        else
        {
            var (factory, exceptionType) = GetFactory(exception.GetType());

            var problemDetailsContextType = typeof(ProblemDetailsContext<>).MakeGenericType(exceptionType);

            var problemDetailsContextInstance = Activator.CreateInstance(
                problemDetailsContextType,
                _problemDetailsFactoryOptions,
                _httpContextAccessor.HttpContext,
                exception,
                builder);

            //var genericContext = new ProblemDetailsContext<TException>(
            //    _problemDetailsFactoryOptions,
            //    _httpContextAccessor.HttpContext,
            //    exception,
            //    builder);

            GetProblemDetails(factory, problemDetailsContextInstance);
        }

        return builder.Build();
    }

    private (object Factory, Type Type) GetFactory(Type? exceptionType)
    {
        ArgumentNullException.ThrowIfNull(exceptionType);

        var factoryType = typeof(IProblemDetailsFactory<>).MakeGenericType(exceptionType);
        var factory = _serviceProvider.GetService(factoryType);

        if (factory is not null)
            return (factory, exceptionType);

        return GetFactory(exceptionType.BaseType);
    }

    private ProblemDetails GetProblemDetails<TException>(object problemDetailsFactory, ProblemDetailsContext<TException> context)
        where TException : Exception
    {
        var problemDetails = problemDetailsFactory
            ?.GetType()
            ?.GetMethod(nameof(IProblemDetailsFactory<TException>.CreateProblemDetails))
            ?.Invoke(problemDetailsFactory, new[] { context });

        if (problemDetails is not null)
            return (ProblemDetails)problemDetails;

        throw new InvalidOperationException(@$"Could not get problem details using factory ""{problemDetailsFactory!.GetType().Name}"" and exception ""{context.GetType().Name}"".");
    }

    private ProblemDetails GetProblemDetails(object problemDetailsFactory, object context)
    {
        var problemDetails = problemDetailsFactory
            ?.GetType()
            ?.GetMethod(nameof(IProblemDetailsFactory<Exception>.CreateProblemDetails))
            ?.Invoke(problemDetailsFactory, new[] { context });

        if (problemDetails is not null)
            return (ProblemDetails)problemDetails;

        throw new InvalidOperationException(@$"Could not get problem details using factory ""{problemDetailsFactory!.GetType().Name}"" and exception ""{context.GetType().Name}"".");
    }
}
