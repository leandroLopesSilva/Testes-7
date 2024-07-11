using Dapper;
using System.Data;

namespace Questao5.Infrastructure.Database;
public interface IIdempotencyRepository
{
    Task<string> GetResultAsync(string requestId);
    Task SaveResultAsync(string requestId, string result);
}

public class IdempotencyRepository : IIdempotencyRepository
{
    private readonly IDbConnection _dbConnection;

    public IdempotencyRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<string> GetResultAsync(string requestId)
    {
        var query = "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @RequestId";
        return await _dbConnection.QuerySingleOrDefaultAsync<string>(query, new { RequestId = requestId });
    }

    public async Task SaveResultAsync(string requestId, string result)
    {
        var query = "INSERT INTO idempotencia (chave_idempotencia, resultado) VALUES (@RequestId, @Result)";
        await _dbConnection.ExecuteAsync(query, new { RequestId = requestId, Result = result });
    }
}
