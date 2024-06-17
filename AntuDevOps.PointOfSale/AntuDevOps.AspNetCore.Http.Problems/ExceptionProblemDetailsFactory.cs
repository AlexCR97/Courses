using Microsoft.Extensions.Configuration;

namespace AntuDevOps.AspNetCore.Http.Problems;

public class ExceptionProblemDetailsFactory : IProblemDetailsFactory<Exception>
{
    public ProblemDetails CreateProblemDetails(ProblemDetailsContext<Exception> context)
    {
        context.Problem
            .WithType("https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1")
            .WithTitle("Unknown")
            .WithStatus(500)
            .WithDetail(context.Exception.Message)
            .WithInstance(context.HttpContext?.Request.Path);

        var options = ToOptions(context.Configuration);

        if (options.IncludeException)
        {
            context.Problem.WithExtension("exception", new
            {
                type = context.Exception.GetType().Name,
                message = context.Exception.Message,
                stackTrace = context.Exception.StackTrace,
            });
        }

        return context.Problem.Build();
    }

    private static Options ToOptions(IConfiguration? configuration)
    {
        var options = Options.Default();
        configuration?.Bind(options);
        return options;
    }

    private record Options(
        bool IncludeException)
    {
        public static Options Default()
        {
            return new Options(
                IncludeException: false);
        }
    }
}
