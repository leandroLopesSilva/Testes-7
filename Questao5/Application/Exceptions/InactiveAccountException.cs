namespace Questao5.Application.Exceptions;

public class InactiveAccountException : BadRequestException
{
    public InactiveAccountException() : base("Inactive account.") { }
}
