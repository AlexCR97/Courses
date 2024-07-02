using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AntuDevOps.AspNetCore.Http.Problems.DependencyInjection;

public interface IProblemDetailsResolverBuilder
{
    public IServiceCollection Services { get; }
    public IConfiguration? Configuration { get; set; }
    public string? ConfigurationSectionName { get; set; }
}

internal class ProblemDetailsResolverBuilder : IProblemDetailsResolverBuilder
{
    public ProblemDetailsResolverBuilder(IServiceCollection services, IConfiguration? configuration, string? configurationSectionName)
    {
        Services = services;
        Configuration = configuration;
        ConfigurationSectionName = configurationSectionName;
    }

    public IServiceCollection Services { get; }
    public IConfiguration? Configuration { get; set; }
    public string? ConfigurationSectionName { get; set; }

    public void SetConfiguration(IConfiguration? configuration, string? sectionName)
    {
        Configuration = configuration;
        ConfigurationSectionName = sectionName;
    }

    public static ProblemDetailsResolverBuilder Create(IServiceCollection services)
    {
        return new ProblemDetailsResolverBuilder(services, null, null);
    }
}

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddProblemDetails(
        this IServiceCollection services,
        Action<IProblemDetailsResolverBuilder> builder)
    {
        var problemDetailsResolverBuilder = ProblemDetailsResolverBuilder.Create(services);

        builder(problemDetailsResolverBuilder);

        problemDetailsResolverBuilder
            .AddProblemDetailsResolver()
            .AddDefaultProblemDetailsFactory();

        return services;
    }

    public static IProblemDetailsResolverBuilder WithConfiguration(this IProblemDetailsResolverBuilder builder, IConfiguration? configuration, string? sectionName = null)
    {
        var specificBuilder = (ProblemDetailsResolverBuilder)builder;
        specificBuilder.SetConfiguration(configuration, sectionName);
        return specificBuilder;
    }

    private static IProblemDetailsResolverBuilder AddProblemDetailsResolver(this IProblemDetailsResolverBuilder builder)
    {
        var sectionName = builder.ConfigurationSectionName ?? "ProblemDetails";

        var problemDetailsConfiguration = builder.Configuration?.GetSection(sectionName);

        builder.Services
            .AddSingleton(new ProblemDetailsResolverOptions(problemDetailsConfiguration))
            .AddSingleton<IProblemDetailsResolver, ProblemDetailsResolver>();

        return builder;
    }

    private static IProblemDetailsResolverBuilder AddDefaultProblemDetailsFactory(this IProblemDetailsResolverBuilder builder)
    {
        return builder.AddProblemDetailsFactory<Exception, ExceptionProblemDetailsFactory>();
    }

    public static IProblemDetailsResolverBuilder AddProblemDetailsFactory<TException, TFactory>(this IProblemDetailsResolverBuilder builder)
        where TException : Exception
        where TFactory : class, IProblemDetailsFactory<TException>
    {
        builder.Services.AddSingleton<IProblemDetailsFactory<TException>, TFactory>();
        return builder;
    }
}
