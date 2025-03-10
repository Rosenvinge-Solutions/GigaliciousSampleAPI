namespace GigaliciousSample.WebAPI.Auth;

public class ApiScope
{
    public Guid Id { get; init; }

    public required string ResourceType { get; init; }

    public required string AccessLevel { get; init; }

    public string Scope => $"{ResourceType}.{AccessLevel}";
}
