using AntuDevOps.AspNetCore.Http.Problems;
using AntuDevOps.PointOfSale.Domain.Exceptions;
using System.Net;

namespace AntuDevOps.PointOfSale.Api.Errors;

internal class NotFoundExceptionProblemDetailsFactory : IProblemDetailsFactory<NotFoundException>
{
    public ProblemDetails CreateProblemDetails(ProblemDetailsContext<NotFoundException> context)
    {
        return context.Problem
            .WithType("https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4")
            .WithTitle(context.Exception.Code)
            .WithStatus((int)HttpStatusCode.NotFound)
            .Build();
    }
}