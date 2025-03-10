using Dapper;
using GigaliciousSample.WebAPI.Auth;
using Microsoft.Data.SqlClient;

namespace GigaliciousSample.WebAPI.Services;

public interface IApiKeyService
{
    Task<ApiKey?> GetApiKeyAsync(string key);

    Task<string> GetScopesByIdAsync(Guid keyId);
}

public class ApiKeyService : IApiKeyService
{
    private readonly string _connectionString;

    public ApiKeyService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<ApiKey?> GetApiKeyAsync(string key)
    {
        using var conn = new SqlConnection(_connectionString);

        var sql = "SELECT * FROM Auth WHERE [Key] = @ApiKey";

        var result = await conn.QuerySingleOrDefaultAsync<ApiKey>(sql, new { ApiKey = key });

        return result;
    }

    public async Task<string> GetScopesByIdAsync(Guid keyId)
    {
        using var conn = new SqlConnection(_connectionString);

        var sql = """
            SELECT 
        	    s.ResourceType,
        	    s.AccessLevel
            FROM Scopes as s
            INNER JOIN Access as aks ON s.Id = aks.Scopes_Id
            WHERE aks.Auth_Id = @ApiKeyId;
        """;

        var scopes = await conn.QueryAsync<ApiScope>(sql, new { ApiKeyId = keyId } );

        return string.Join(" ", scopes.Select(s => s.Scope));
    }
}
