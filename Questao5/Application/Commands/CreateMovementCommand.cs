using MediatR;

namespace Questao5.Application.Commands;

public class CreateMovementCommand : IRequest<string>
{
    public string RequestId { get; set; }
    public string AccountId { get; set; }
    public decimal Amount { get; set; }
    public string MovementType { get; set; } // "C" for Credit, "D" for Debit
}
