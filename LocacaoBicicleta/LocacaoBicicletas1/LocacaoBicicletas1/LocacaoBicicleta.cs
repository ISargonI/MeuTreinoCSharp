using System;

public class LocacaoBicicleta 
{
    public string NomeCliente;
    public string ModeloBicicleta;
    public int QuantidadeHoras;
    public decimal ValorHora;

    public decimal CalcularTotal() 
    {
        return QuantidadeHoras * ValorHora;
    }

    public void ExibirDados() 
    {
        Console.WriteLine($"Cliente: {NomeCliente}");
        Console.WriteLine($"Bicicleta: {ModeloBicicleta}");
        Console.WriteLine($"Horas: {QuantidadeHoras}");
        Console.WriteLine($"Valor por hora: {ValorHora:C}");
        Console.WriteLine($"Total a pagar: {CalcularTotal():C}");
        Console.WriteLine("-----------------------------");
    }
}