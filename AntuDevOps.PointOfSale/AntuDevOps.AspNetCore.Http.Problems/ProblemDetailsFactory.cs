using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AntuDevOps.AspNetCore.Http.Problems;

public interface IProblemDetailsFactory<TException>
    where TException : Exception
{
    ProblemDetails CreateProblemDetails(ProblemDetailsContext<TException> context);
}

public record ProblemDetailsContext<TException>(
    IConfiguration? Configuration,
    HttpContext? HttpContext,
    TException Exception,
    ProblemDetailsBuilder Problem);
