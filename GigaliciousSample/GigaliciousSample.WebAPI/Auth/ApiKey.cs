namespace GigaliciousSample.WebAPI.Auth;

public class ApiKey
{
    public Guid Id { get; init; }

    public required string Key { get; init; }

    public required DateTime CreatedAt { get; init; }
}
