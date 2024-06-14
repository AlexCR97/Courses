using AntuDevOps.PointOfSale.Domain.Exceptions;
using System.Net;

namespace AntuDevOps.PointOfSale.Api.Errors;

public record ErrorResponse(
    HttpStatusCode Status,
    string Code,
    string Message,
    string? StackTrace);

public interface IErrorResponseParser
{
    ErrorResponse Parse(Exception exception);
}

public record ErrorResponseParserOptions(
    bool IncludeStackTrace)
{
    public static ErrorResponseParserOptions Default()
    {
        return new ErrorResponseParserOptions(false);
    }
}

internal class ErrorResponseParser : IErrorResponseParser
{
    private readonly ErrorResponseParserOptions _options;

    public ErrorResponseParser(ErrorResponseParserOptions options)
    {
        _options = options;
    }

    public ErrorResponse Parse(Exception exception)
    {
        // SOLID
        // O -> Open-Closed principle
        // A component should be Open for extension, but Closed for modification

        HttpStatusCode status = exception switch
        {
            NotFoundException _ => HttpStatusCode.NotFound,
            DomainException _ => HttpStatusCode.BadRequest,
            Exception _ => HttpStatusCode.InternalServerError,
            _ => HttpStatusCode.InternalServerError,
        };

        return new ErrorResponse(
            status,
            exception.GetType().Name,
            exception.Message,
            _options.IncludeStackTrace
                ? exception.StackTrace
                : null);
    }
}

internal static class ErrorResponseParserDependencyInjection
{
    public static IServiceCollection AddErrorResponse(this IServiceCollection services, IConfiguration configuration)
    {
        var options = ErrorResponseParserOptions.Default();

        if (configuration.GetSection("ErrorResponse") is not null)
            configuration.Bind("ErrorResponse", options);

        services.AddSingleton(options);

        services.AddSingleton<IErrorResponseParser, ErrorResponseParser>();

        return services;
    }
}
