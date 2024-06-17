using Microsoft.AspNetCore.Http;

namespace AntuDevOps.AspNetCore.Http.Problems;

public interface IProblemDetailsFactory;

public interface IProblemDetailsFactory<TException> : IProblemDetailsFactory
    where TException : Exception
{
    ProblemDetails CreateProblemDetails(ProblemDetailsContext<TException> context);
}

public record ProblemDetailsContext<TException>(
    ProblemDetailsFactoryOptions Options,
    HttpContext? HttpContext,
    TException Exception,
    ProblemDetailsBuilder Problem);

public record ProblemDetailsFactoryOptions(
    bool IncludeException)
{
    public static ProblemDetailsFactoryOptions Default()
    {
        return new ProblemDetailsFactoryOptions(
            IncludeException: false);
    }
}
