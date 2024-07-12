namespace Questao5.UnitTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Test1()
    {
        MovementControllerTests movementControllerTests = new MovementControllerTests();
        movementControllerTests.CreateMovementTests();
        Assert.Pass();
    }
}