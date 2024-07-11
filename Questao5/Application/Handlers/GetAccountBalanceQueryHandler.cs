using MediatR;
using Questao5.Application.DTOs;
using Questao5.Application.Exceptions;
using Questao5.Application.Queries;
using Questao5.Infrastructure.Database;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Application.Handlers;

public class GetAccountBalanceQueryHandler : IRequestHandler<GetAccountBalanceQuery, AccountBalanceDto>
{
    private readonly IDapperContext _context;

    public GetAccountBalanceQueryHandler(IDapperContext context)
    {
        _context = context;
    }

    public async Task<AccountBalanceDto> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
    {
        // Validate account
        var account = await _context.GetAccountByIdAsync(request.AccountId);
        if (account == null)
        {
            throw new InvalidAccountException();
        }
        if (!account.Ativo)
        {
            throw new InactiveAccountException();
        }

        // Calculate balance
        var movements = await _context.GetMovementsByAccountIdAsync(request.AccountId);
        var credits = movements.Where(m => m.TipoMovimento == "C").Sum(m => m.Valor);
        var debits = movements.Where(m => m.TipoMovimento == "D").Sum(m => m.Valor);
        var balance = credits - debits;

        return new AccountBalanceDto
        {
            NumeroContaCorrente = account.Numero,
            NomeTitular = account.Nome,
            DataHoraConsulta = DateTime.Now,
            Saldo = balance
        };
    }
}
