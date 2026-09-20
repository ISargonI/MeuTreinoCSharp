using System;

public class RegistroEnergia
{
    public string NomeResponsavel;
    public string MesReferencia;
    public double ConsumoKwh;
    public decimal ValorKwh;

    public decimal CalcularValorConta()
    {
        return (decimal)ConsumoKwh * ValorKwh;
    }

    // Método que retorna um valor verdadeiro ou falso (bool)
    public bool ConsumoAlto()
    {
        return ConsumoKwh > 250;
    }

    public void ExibirDados()
    {
        Console.WriteLine($"Responsável: {NomeResponsavel}");
        Console.WriteLine($"Mês: {MesReferencia}");
        Console.WriteLine($"Consumo: {ConsumoKwh} kWh");
        Console.WriteLine($"Valor cobrado por kWh: {ValorKwh:C}");
        Console.WriteLine($"Valor total da conta: {CalcularValorConta():C}");
        
        // Usa o método ConsumoAlto() para imprimir Sim ou Não
        Console.WriteLine($"Consumo foi alto? {(ConsumoAlto() ? "Sim" : "Não")}");
        Console.WriteLine("-----------------------------");
    }
}