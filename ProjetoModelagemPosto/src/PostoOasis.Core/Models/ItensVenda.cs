namespace PostoOasis.Core.Models;

// Conceito POO: Composição - Parte integrante de uma Venda
public class ItemVenda
{
    public Produto Produto { get; set; } = null!;
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal Subtotal => Quantidade * PrecoUnitario;
}

public class ItemCombustivel
{
    public Bomba Bomba { get; set; } = null!;
    public double Litros { get; set; }
    public decimal PrecoLitro { get; set; }
    public decimal Subtotal => (decimal)Litros * PrecoLitro;
}
