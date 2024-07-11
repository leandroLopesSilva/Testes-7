namespace Questao5.Domain.Entities;

public class ContaCorrente
{
    public string IdContaCorrente { get; set; } // ID da conta corrente
    public int Numero { get; set; } // Número da conta corrente
    public string Nome { get; set; } // Nome do titular da conta corrente
    public bool Ativo { get; set; } // Indicativo se a conta está ativa (0 = inativa, 1 = ativa)

    public ContaCorrente()
    {
    }

    public ContaCorrente(string idContaCorrente, int numero, string nome, bool ativo)
    {
        IdContaCorrente = idContaCorrente;
        Numero = numero;
        Nome = nome;
        Ativo = ativo;
    }
}
