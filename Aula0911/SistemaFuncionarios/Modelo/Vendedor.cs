using System;
using System.Collections.Generic;

public class Vendedor : Funcionario
{
    public double PercentualComissao { get; private set; }
    public double TotalVendasMes { get; private set; }

    public Vendedor(string nome, string matricula, double salarioBase, double percentual, double totalVendas) 
        : base(nome, matricula, salarioBase)
    {
        if (percentual < 0) throw new ArgumentOutOfRangeException("Percentual inválido.");
        if (totalVendas < 0) throw new ArgumentOutOfRangeException("Vendas inválidas.");

        PercentualComissao = percentual;
        TotalVendasMes = totalVendas;
    }

    public double CalcularComissao() => TotalVendasMes * (PercentualComissao / 100.0);

    public override void ExibirDados()
    {
        base.ExibirDados();
        Console.WriteLine($"Comissão: R$ {CalcularComissao():F2}");
    }
}