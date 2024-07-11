namespace Questao5.Application.DTOs;

public class AccountBalanceDto
{
    public int NumeroContaCorrente { get; set; }
    public string NomeTitular { get; set; }
    public DateTime DataHoraConsulta { get; set; }
    public decimal Saldo { get; set; }
}
