using System;
class ContaBancaria
{
    private readonly int numeroConta;
    private string titularConta;
    private double saldo;

    public double Saldo
    {
        get { return saldo; }
    }

    public string TitularConta
    {
        get { return titularConta; }
        set { titularConta = value; }
    }

    public int NumeroConta
    {
        get { return numeroConta; }
    }
    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="numeroConta"></param>
    /// <param name="titularConta"></param>
    /// <param name="saldoInicial"></param>
    public ContaBancaria(int numeroConta, string titularConta, double saldoInicial = 0)
    {
        this.numeroConta = numeroConta;
        this.titularConta = titularConta;
        this.saldo = saldoInicial;
    }
    /// <summary>
    /// Método para depósito
    /// </summary>
    /// <param name="valor"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Deposito(double valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor do depósito deve ser maior que zero.", nameof(valor));
        }
        saldo += valor;
    }
    /// <summary>
    /// Método para saque
    /// </summary>
    /// <param name="valor"></param>
    /// <exception cref="ArgumentException"></exception>
    public void Saque(double valor)
    {
        if (valor <= 0)
        {
            throw new ArgumentException("O valor do saque deve ser maior que zero.", nameof(valor));
        }

        double taxaSaque = 3.50;
        double valorTotalSaque = valor + taxaSaque;

        saldo -= valorTotalSaque; // permitir saldo negativo
    }
    /// <summary>
    /// Override para formatar a saída dos dados da conta
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return $"Conta {numeroConta}, Titular: {titularConta}, Saldo: ${saldo:F2}";
    }
}
