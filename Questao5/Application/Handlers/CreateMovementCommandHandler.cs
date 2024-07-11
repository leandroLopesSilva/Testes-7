using MediatR;
using Questao5.Application.Commands;
using Questao5.Application.Exceptions;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database;
using System.ComponentModel.DataAnnotations;

namespace Questao5.Application.Handlers;

public class CreateMovementCommandHandler : IRequestHandler<CreateMovementCommand, string>
{
    private readonly IDapperContext _context;
    private readonly IIdempotencyRepository _idempotencyRepository;

    public CreateMovementCommandHandler(IDapperContext context, IIdempotencyRepository idempotencyRepository)
    {
        _context = context;
        _idempotencyRepository = idempotencyRepository;
    }

    public async Task<string> Handle(CreateMovementCommand request, CancellationToken cancellationToken)
    {
        // Check idempotency
        var existingResult = await _idempotencyRepository.GetResultAsync(request.RequestId);
        if (existingResult != null)
        {
            return existingResult;
        }

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

        // Validate amount
        if (request.Amount <= 0)
        {
            throw new InvalidValueException();
        }

        // Validate movement type
        if (request.MovementType != "C" && request.MovementType != "D")
        {
            throw new InvalidTypeException();
        }

        // Create movement
        var movement = new Movimento
        {
            IdMovimento = Guid.NewGuid().ToString(),
            IdContaCorrente = request.AccountId,
            DataMovimento = DateTime.Now.ToString("dd/MM/yyyy"),
            TipoMovimento = request.MovementType,
            Valor = request.Amount
        };

        await _context.CreateMovementAsync(movement);

        // Save idempotency
        await _idempotencyRepository.SaveResultAsync(request.RequestId, movement.IdMovimento);

        return movement.IdMovimento;
    }
}
