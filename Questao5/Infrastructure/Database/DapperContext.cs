using System.Data;
using Dapper;
using global::Questao5.Infrastructure.Sqlite;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
namespace Questao5.Infrastructure.Database;

public class DapperContext : IDapperContext
{
    private readonly DatabaseConfig _databaseConfig;

    public DapperContext(DatabaseConfig databaseConfig)
    {
        _databaseConfig = databaseConfig;
    }

    public IDbConnection CreateConnection()
    {
        return new SqliteConnection(_databaseConfig.Name);
    }

    public async Task<IEnumerable<Movimento>> GetMovementsByAccountIdAsync(string accountId)
    {
        using (var connection = CreateConnection())
        {
            string sql = "SELECT * FROM movimento WHERE idcontacorrente = @AccountId";
            return await connection.QueryAsync<Movimento>(sql, new { AccountId = accountId });
        }
    }

    public async Task<ContaCorrente> GetAccountByIdAsync(string accountId)
    {
        using (var connection = CreateConnection())
        {
            string sql = "SELECT * FROM contacorrente WHERE numero = @AccountId";
            return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(sql, new { AccountId = accountId });
        }
    }

    public async Task CreateMovementAsync(Movimento movement)
    {
        using (var connection = CreateConnection())
        {
            string sql = @"
                    INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor)
                    VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)";
            await connection.ExecuteAsync(sql, movement);
        }
    }
}