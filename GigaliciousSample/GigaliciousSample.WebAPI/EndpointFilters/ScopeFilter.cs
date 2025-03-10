namespace GigaliciousSample.WebAPI.EndpointFilters;

public class ScopeFilter : IEndpointFilter
{
    private readonly string _requiredScope;

    public ScopeFilter(string requiredScope)
    {
        _requiredScope = requiredScope;

    }

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        if (context.HttpContext.Items["Scopes"] is not string scopes || !scopes.Contains(_requiredScope))
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.HttpContext.Response.WriteAsync("You do not have the required scope to access this endpoint");
        }

        return await next(context);
    }
}
