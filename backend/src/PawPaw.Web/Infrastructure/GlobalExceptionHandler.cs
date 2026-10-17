using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace PawPaw.Web.Infrastructure;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
  public async ValueTask<bool> TryHandleAsync(
    HttpContext httpContext,
    Exception exception,
    CancellationToken cancellationToken)
  {
    logger.LogError(exception, "Unhandled exception while processing {Path}", httpContext.Request.Path);

    var problem = new ProblemDetails
    {
      Status = StatusCodes.Status500InternalServerError,
      Title = "An unexpected error occurred.",
      Detail = "The request could not be completed.",
      Instance = httpContext.Request.Path
    };

    problem.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;

    httpContext.Response.StatusCode = problem.Status.Value;
    await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
    return true;
  }
}
