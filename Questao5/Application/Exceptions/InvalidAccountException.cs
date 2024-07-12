namespace Questao5.Application.Exceptions;

public class InvalidAccountException : BadRequestException
{
    public InvalidAccountException() : base("INVALID_ACCOUNT") { }
}
