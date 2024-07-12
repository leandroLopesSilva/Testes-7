using Questao5.Application.Commands;
using Questao5.Application.Exceptions;
using Questao5.Infrastructure.Database;

namespace Questao5.Tests.Application.Handlers;

public class CreateMovementCommandHandlerTests
{
    private readonly IDapperContext _dapperContext;
    private readonly IIdempotencyRepository _idempotencyRepository;
    private readonly CreateMovementCommandHandler _handler;

    public CreateMovementCommandHandlerTests()
    {
        _dapperContext = Substitute.For<IDapperContext>();
        _idempotencyRepository = Substitute.For<IIdempotencyRepository>();
        _handler = new CreateMovementCommandHandler(_dapperContext, _idempotencyRepository);
    }

    [Fact]
    public async Task Handle_ShouldReturnExistingResult_WhenRequestIdIsAlreadyProcessed()
    {
        // Arrange - Ajustar para dados que já estão na base
        var requestId = "0596782"; 
        var existingResult = "4acea7b3-7a10-482e-b802-b615d3bbbd00";

        var account = new ContaCorrente
        {
            IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
            Numero = 456,
            Nome = "Eva Woodward",
            Ativo = true
        };

        _dapperContext.GetAccountByIdAsync(account.IdContaCorrente).Returns(Task.FromResult(account));

        _idempotencyRepository.GetResultAsync(requestId).Returns(Task.FromResult(existingResult));

        var command = new CreateMovementCommand
        {
            RequestId = requestId,
            AccountId = account.IdContaCorrente,
            Amount = 100,
            MovementType = "C"
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(existingResult);
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidAccountException_WhenAccountIsNotFound()
    {
        // Arrange
        var command = new CreateMovementCommand
        {
            RequestId = Guid.NewGuid().ToString(),
            AccountId = "5986",
            Amount = 100m,
            MovementType = "C"
        };

        _dapperContext.GetAccountByIdAsync(command.AccountId).Returns(Task.FromResult<ContaCorrente>(null));

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidAccountException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowInactiveAccountException_WhenAccountIsInactive()
    {
        // Arrange
        var account = new ContaCorrente
        {
            IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
            Numero = 741,
            Nome = "Ameena Lynn",
            Ativo = false
        };

        _dapperContext.GetAccountByIdAsync(account.IdContaCorrente).Returns(Task.FromResult(account));

        var command = new CreateMovementCommand
        {
            RequestId = Guid.NewGuid().ToString(),
            AccountId = account.IdContaCorrente,
            Amount = 100m,
            MovementType = "C"
        };

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InactiveAccountException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidValueException_WhenAmountIsNotPositive()
    {
        // Arrange
        var account = new ContaCorrente
        {
            IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
            Numero = 456,
            Nome = "Eva Woodward",
            Ativo = true
        };

        _dapperContext.GetAccountByIdAsync(account.IdContaCorrente).Returns(Task.FromResult(account));

        var command = new CreateMovementCommand
        {
            RequestId = Guid.NewGuid().ToString(),
            AccountId = account.IdContaCorrente,
            Amount = -100,
            MovementType = "C"
        };

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidValueException>();
    }

    [Fact]
    public async Task Handle_ShouldThrowInvalidTypeException_WhenMovementTypeIsInvalid()
    {
        // Arrange
        var account = new ContaCorrente
        {
            IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
            Numero = 456,
            Nome = "Eva Woodward",
            Ativo = true
        };

        _dapperContext.GetAccountByIdAsync(account.IdContaCorrente).Returns(Task.FromResult(account));

        // Arrange
        var command = new CreateMovementCommand
        {
            RequestId = Guid.NewGuid().ToString(),
            AccountId = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
            Amount = 100m,
            MovementType = "X"
        };

        // Act & Assert
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<InvalidTypeException>();
    }

    [Fact]
    public async Task Handle_ShouldCreateMovement_WhenRequestIsValid()
    {
        // Arrange
        var account = new ContaCorrente
        {
            IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9",
            Numero = 456,
            Nome = "Eva Woodward",
            Ativo = true
        };

        _dapperContext.GetAccountByIdAsync(account.IdContaCorrente).Returns(Task.FromResult(account));

        var command = new CreateMovementCommand
        {
            RequestId = Guid.NewGuid().ToString(),
            AccountId = account.IdContaCorrente,
            Amount = 100m,
            MovementType = "C"
        };

        _idempotencyRepository.GetResultAsync(command.RequestId).Returns(Task.FromResult<string>(null));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        await _dapperContext.Received().CreateMovementAsync(Arg.Is<Movimento>(m =>
            m.IdContaCorrente == account.IdContaCorrente &&
            m.TipoMovimento == "C" &&
            m.Valor == 100m));
        await _idempotencyRepository.Received().SaveResultAsync(command.RequestId, result);
    }
}