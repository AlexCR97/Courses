using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AntuDevOps.AspNetCore.Http.Problems.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddProblemDetails(
        this IServiceCollection services,
        IConfiguration configuration,
        string? sectionName = null)
    {
        return services
            .AddProblemDetailsResolverOptions(configuration, sectionName)
            .AddSingleton<IProblemDetailsResolver, ProblemDetailsResolver>()
            .AddProblemDetailsFactory<Exception, ExceptionProblemDetailsFactory>();
    }

    private static IServiceCollection AddProblemDetailsResolverOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        string? sectionName = null)
    {
        sectionName ??= "ProblemDetails";

        var problemDetailsConfiguration = configuration.GetSection(sectionName);

        return services
            .AddSingleton(new ProblemDetailsResolverOptions(problemDetailsConfiguration));
    }

    public static IServiceCollection AddProblemDetailsFactory<TException, TFactory>(this IServiceCollection services)
        where TException : Exception
        where TFactory : class, IProblemDetailsFactory<TException>
    {
        return services
            .AddSingleton<IProblemDetailsFactory<TException>, TFactory>();
    }
}
