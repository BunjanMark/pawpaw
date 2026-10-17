using System.Threading.RateLimiting;
using FastEndpoints;
using FastEndpoints.Swagger;
using PawPaw.Web.Infrastructure;
using PawPaw.Web.Security;

namespace PawPaw.Web.Configurations;

public static class WebServiceConfiguration
{
  public const string CorsPolicyName = "Frontend";

  public static IServiceCollection AddPawPawWeb(
    this IServiceCollection services,
    IConfiguration configuration)
  {
    services.AddProblemDetails();
    services.AddExceptionHandler<GlobalExceptionHandler>();
    services.AddHealthChecks();

    services.AddAuthorizationBuilder()
      .AddPolicy(AuthorizationPolicies.AuthenticatedUser, policy =>
        policy.RequireAuthenticatedUser())
      .AddPolicy(AuthorizationPolicies.Administrator, policy =>
        policy.RequireAuthenticatedUser().RequireRole("Admin"))
      .AddPolicy(AuthorizationPolicies.Veterinarian, policy =>
        policy.RequireAuthenticatedUser().RequireRole("Veterinarian"));

    var allowedOrigins = configuration
      .GetSection("Cors:AllowedOrigins")
      .GetChildren()
      .Select(section => section.Value)
      .OfType<string>()
      .Where(origin => !string.IsNullOrWhiteSpace(origin))
      .ToArray();

    services.AddCors(options =>
    {
      options.AddPolicy(CorsPolicyName, policy =>
      {
        if (allowedOrigins.Length > 0)
        {
          policy.WithOrigins(allowedOrigins);
        }
        else
        {
          policy.SetIsOriginAllowed(_ => false);
        }

        policy.AllowAnyHeader().AllowAnyMethod();
      });
    });

    services.AddRateLimiter(options =>
    {
      options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
      options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
          context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
          _ => new FixedWindowRateLimiterOptions
          {
            PermitLimit = 200,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
            AutoReplenishment = true
          }));
    });

    services
      .AddFastEndpoints()
      .SwaggerDocument(options =>
      {
        options.DocumentSettings = settings =>
        {
          settings.Title = "PawPaw API";
          settings.Version = "v1";
          settings.Description = "REST API for the PawPaw pet application.";
        };
        options.ShortSchemaNames = true;
      });

    return services;
  }
}
