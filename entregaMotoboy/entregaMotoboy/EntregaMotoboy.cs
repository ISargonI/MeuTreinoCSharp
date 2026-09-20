using System;

public class EntregaMotoboy
{
    public string Destinatario;
    public double DistanciaKm;
    public decimal ValorBase;
    public decimal ValorAdicionalKm;

    // Calcula combinando a taxa fixa e o valor por quilômetro rodado
    public decimal CalcularValorFinal()
    {
        return ValorBase + ((decimal)DistanciaKm * ValorAdicionalKm);
    }

    // Método que verifica se a entrega é longa
    public bool EntregaLongaDistancia()
    {
        return DistanciaKm > 15;
    }

    public void ExibirDados()
    {
        Console.WriteLine($"Destinatário: {Destinatario}");
        Console.WriteLine($"Distância: {DistanciaKm} km");
        Console.WriteLine($"Taxa Base: {ValorBase:C}");
        Console.WriteLine($"Adicional por km: {ValorAdicionalKm:C}");
        Console.WriteLine($"Valor total da entrega: {CalcularValorFinal():C}");
        
        // Exibe se é longa distância usando o método booleano
        Console.WriteLine($"É longa distância? {(EntregaLongaDistancia() ? "Sim" : "Não")}");
        Console.WriteLine("-----------------------------");
    }
}