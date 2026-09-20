using System;

public class ViagemAplicativo
{
    public string NomePassageiro;
    public double DistanciaKm;
    public decimal ValorPorKm;
    public decimal TaxaFixa;

    // O documento fornece exatamente esta lógica para o cálculo
    public decimal CalcularPreco()
    {
        return TaxaFixa + ((decimal)DistanciaKm * ValorPorKm);
    }

    // Informa se a viagem foi longa (mais de 20 km)[cite: 1]
    public bool ViagemLonga()
    {
        return DistanciaKm > 20;
    }

    // Exibe o resumo da viagem[cite: 1]
    public void ExibirResumo()
    {
        Console.WriteLine($"Passageiro: {NomePassageiro}");
        Console.WriteLine($"Distância: {DistanciaKm} km");
        Console.WriteLine($"Taxa Fixa: {TaxaFixa:C}");
        Console.WriteLine($"Valor por km: {ValorPorKm:C}");
        Console.WriteLine($"Preço final da viagem: {CalcularPreco():C}");
        Console.WriteLine($"Viagem longa? {(ViagemLonga() ? "Sim" : "Não")}");
        Console.WriteLine("-----------------------------");
    }
}