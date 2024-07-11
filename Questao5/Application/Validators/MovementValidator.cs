using FluentValidation;
using Questao5.Application.Commands;

namespace Questao5.Application.Validators;

public class MovementValidator : AbstractValidator<CreateMovementCommand>
{
    public MovementValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty().WithMessage("AccountId is required.");
        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        RuleFor(x => x.MovementType).Must(x => x == "C" || x == "D").WithMessage("Invalid movement type.");
    }
}
