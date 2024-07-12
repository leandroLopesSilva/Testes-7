using NSubstitute;
using Questao5.Application.Commands;
using Questao5.Domain.Entities;
using Xunit;

namespace Questao5.UnitTests;
public class MovementControllerTests
{
    [Fact]
    public void CreateMovementTests()
    {
        var creatMovimento = Substitute.For<CreateMovementCommand>();
        creatMovimento.Returns(creatMovimento);
    }

    //[Test]
    //public void Test1()
    //{
    //    Assert.Pass();
    //}
}