using System;

public class PedidoImpressao
{
    public string NomeCliente;
    public int QuantidadePaginas;
    public int NumeroCopias;
    public decimal PrecoPagina;

    // Primeiro método: calcula o total de páginas geradas
    public int CalcularTotalPaginas()
    {
        return QuantidadePaginas * NumeroCopias;
    }

    // Segundo método: usa o resultado do primeiro para calcular o dinheiro
    public decimal CalcularValorTotal()
    {
        return CalcularTotalPaginas() * PrecoPagina;
    }

    public void ExibirResumo()
    {
        Console.WriteLine($"Cliente: {NomeCliente}");
        Console.WriteLine($"Páginas do documento: {QuantidadePaginas}");
        Console.WriteLine($"Número de cópias: {NumeroCopias}");
        Console.WriteLine($"Total de páginas impressas: {CalcularTotalPaginas()}");
        Console.WriteLine($"Preço por página: {PrecoPagina:C}");
        Console.WriteLine($"Valor total do pedido: {CalcularValorTotal():C}");
        Console.WriteLine("-----------------------------");
    }
}