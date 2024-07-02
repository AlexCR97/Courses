using AntuDevOps.AspNetCore.Http.Problems;
using AntuDevOps.PointOfSale.Domain.Exceptions;
using System.Net;

namespace AntuDevOps.PointOfSale.Api.Errors;

internal class DomainExceptionProblemDetailsFactory : IProblemDetailsFactory<DomainException>
{
    public ProblemDetails CreateProblemDetails(ProblemDetailsContext<DomainException> context)
    {
        return context.Problem
            .WithType("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1")
            .WithTitle(context.Exception.Code)
            .WithStatus((int)HttpStatusCode.BadRequest)
            .Build();
    }
}
