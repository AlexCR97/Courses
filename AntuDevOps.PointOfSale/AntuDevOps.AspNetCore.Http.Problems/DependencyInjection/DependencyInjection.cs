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
            .AddProblemDetailsFactoryOptions(configuration, sectionName)
            .AddProblemDetailsFactory<Exception, ExceptionProblemDetailsFactory>()
            .AddSingleton<IProblemDetailsResolver, ProblemDetailsResolver>();
    }

    private static IServiceCollection AddProblemDetailsFactoryOptions(
        this IServiceCollection services,
        IConfiguration configuration,
        string? sectionName = null)
    {
        sectionName ??= "ProblemDetails";

        var options = ProblemDetailsFactoryOptions.Default();

        if (configuration.GetSection(sectionName) is not null)
            configuration.Bind(sectionName, options);

        return services.AddSingleton(options);
    }

    public static IServiceCollection AddProblemDetailsFactory<TException, TFactory>(this IServiceCollection services)
        where TException : Exception
        where TFactory : class, IProblemDetailsFactory, IProblemDetailsFactory<TException>
    {
        return services
            .AddSingleton<IProblemDetailsFactory, TFactory>()
            .AddSingleton<IProblemDetailsFactory<TException>, TFactory>();
    }
}
