using Questao5.Application.Exceptions;
using Questao5.Application.Queries;
using Questao5.Infrastructure.Database;

namespace Questao5.Tests.Handlers
{
    public class GetAccountBalanceQueryHandlerTests
    {
        private readonly IDapperContext _dapperContext;
        private readonly GetAccountBalanceQueryHandler _handler;

        public GetAccountBalanceQueryHandlerTests()
        {
            _dapperContext = Substitute.For<IDapperContext>();
            _handler = new GetAccountBalanceQueryHandler(_dapperContext);
        }

        [Fact]
        public async Task Handle_ShouldReturnCorrectBalance_WhenAccountIsValid()
        {
            // Arrange
            var accountId = "valid-id";
            var account = new ContaCorrente
            {
                IdContaCorrente = accountId,
                Numero = 123,
                Nome = "Teste",
                Ativo = true
            };

            var movements = new[]
            {
                new Movimento { TipoMovimento = "C", Valor = 100m },
                new Movimento { TipoMovimento = "D", Valor = 50m }
            };

            _dapperContext.GetAccountByIdAsync(accountId).Returns(account);
            _dapperContext.GetMovementsByAccountIdAsync(accountId).Returns(Task.FromResult(movements.AsEnumerable()));

            var query = new GetAccountBalanceQuery { AccountId = accountId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.NumeroContaCorrente.Should().Be(123);
            result.NomeTitular.Should().Be("Teste");
            result.Saldo.Should().Be(50m);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidAccountException_WhenAccountIsNotFound()
        {
            // Arrange
            var accountId = "invalid-id";
            var query = new GetAccountBalanceQuery { AccountId = accountId };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidAccountException>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowInactiveAccountException_WhenAccountIsInactive()
        {
            // Arrange
            var accountId = "inactive-id";
            var account = new ContaCorrente
            {
                IdContaCorrente = accountId,
                Numero = 123,
                Nome = "Teste",
                Ativo = false
            };

            _dapperContext.GetAccountByIdAsync(accountId).Returns(account);

            var query = new GetAccountBalanceQuery { AccountId = accountId };

            // Act & Assert
            await Assert.ThrowsAsync<InactiveAccountException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
