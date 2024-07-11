namespace Questao5.Domain.Entities;

public class Movimento
{
    public string IdMovimento { get; set; } // Identificação única do movimento
    public string IdContaCorrente { get; set; } // Identificação única da conta corrente
    public string DataMovimento { get; set; } // Data do movimento no formato DD/MM/YYYY
    public string TipoMovimento { get; set; } // Tipo do movimento (C = Crédito, D = Débito)
    public decimal Valor { get; set; } // Valor do movimento, usando duas casas decimais

    public Movimento()
    {
    }

    public Movimento(string idMovimento, string idContaCorrente, string dataMovimento, string tipoMovimento, decimal valor)
    {
        IdMovimento = idMovimento;
        IdContaCorrente = idContaCorrente;
        DataMovimento = dataMovimento;
        TipoMovimento = tipoMovimento;
        Valor = valor;
    }
}
