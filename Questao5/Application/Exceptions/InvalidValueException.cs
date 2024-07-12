namespace Questao5.Application.Exceptions;

public class InvalidValueException : BadRequestException
{
    public InvalidValueException() : base("INVALID_VALUE") { }
}
