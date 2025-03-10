namespace GigaliciousSample.WebAPI.EndpointFilters.Extensions;

public static class EndpointRouteBuilderExtensions
{
    public static RouteHandlerBuilder RequireScope(this RouteHandlerBuilder builder, string scope)
    {
        return builder.AddEndpointFilter(new ScopeFilter(scope));
    }
}
