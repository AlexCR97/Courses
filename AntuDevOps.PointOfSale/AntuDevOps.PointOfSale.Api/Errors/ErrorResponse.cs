using AntuDevOps.PointOfSale.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace AntuDevOps.PointOfSale.Api.Errors;

public record ErrorResponse(
    HttpStatusCode Status,
    string Code,
    string Message,
    string? StackTrace);

public interface IErrorResponseParser
{
    ProblemDetails Parse(Exception exception);
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
    private readonly IHttpContextAccessor _contextAccessor; // Add this line!

    public ErrorResponseParser(ErrorResponseParserOptions options, IHttpContextAccessor contextAccessor)
    {
        _options = options;
        _contextAccessor = contextAccessor;
    }

    public ProblemDetails Parse(Exception exception)
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

        var problemDetails = new ProblemDetails
        {
            Type = "https://developer.mozilla.org/en-US/docs/Web/HTTP/Status",
            Title = exception.GetType().Name,
            Status = (int)status,
            Detail = exception.Message,
            Instance = _contextAccessor.HttpContext?.Request.Path,
        };

        if (_options.IncludeStackTrace)
            problemDetails.Extensions["stackTrace"] = exception.StackTrace;

        return problemDetails;
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
