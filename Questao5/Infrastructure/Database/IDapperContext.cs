using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Database;

public interface IDapperContext
{
    IDbConnection CreateConnection();
    Task<IEnumerable<Movimento>> GetMovementsByAccountIdAsync(string accountId);
    Task<ContaCorrente> GetAccountByIdAsync(string accountId);
    Task CreateMovementAsync(Movimento movement);
}
