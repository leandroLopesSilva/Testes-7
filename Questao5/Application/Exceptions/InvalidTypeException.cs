namespace Questao5.Application.Exceptions;

public class InvalidTypeException : BadRequestException
{
    public InvalidTypeException() : base("INVALID_TYPE") { }
}