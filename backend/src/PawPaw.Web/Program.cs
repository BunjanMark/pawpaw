using FastEndpoints;
using FastEndpoints.Swagger;
using PawPaw.Infrastructure;
using PawPaw.Web.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddJsonConsole();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddPawPawWeb(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();
app.UseCors(WebServiceConfiguration.CorsPolicyName);
app.UseRateLimiter();
app.UseAuthorization();

app.UseFastEndpoints(options =>
{
  options.Endpoints.RoutePrefix = "api/v1";
});

if (app.Environment.IsDevelopment())
{
  app.UseSwaggerGen();
}

app.MapHealthChecks("/health");
app.MapGet("/api/v1/status", () => Results.Ok(new
{
  service = "PawPaw API",
  version = "v1",
  status = "ok"
}));

app.Run();

public partial class Program;
