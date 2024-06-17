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

        if (context.Options.IncludeException)
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
}
