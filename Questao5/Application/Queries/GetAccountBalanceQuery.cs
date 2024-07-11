using MediatR;
using Questao5.Application.DTOs;

namespace Questao5.Application.Queries;

public class GetAccountBalanceQuery : IRequest<AccountBalanceDto>
{
    public string AccountId { get; set; }
}
