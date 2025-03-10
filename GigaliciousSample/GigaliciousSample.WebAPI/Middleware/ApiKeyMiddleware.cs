using GigaliciousSample.WebAPI.Services;
using System.Linq;

namespace GigaliciousSample.WebAPI.Middleware;

public class ApiKeyMiddleware
{
    private const string API_KEY_HEADER = "X-Api-Key";

    private readonly RequestDelegate _next;

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IApiKeyService apiKeyService)
    {
        if (!context.Request.Headers.TryGetValue(API_KEY_HEADER, out var apiKeyHeaderValues))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }
        var apiKey = apiKeyHeaderValues.FirstOrDefault();

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("API Key is missing");
            return;
        }
        var apiKeyEntity = await apiKeyService.GetApiKeyAsync(apiKey);
        if (apiKeyEntity is null)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid API Key");
            return;
        }

        var scopes = await apiKeyService.GetScopesByIdAsync(apiKeyEntity.Id);

        context.Items["Scopes"] = scopes;

        await _next(context);
    }
}
