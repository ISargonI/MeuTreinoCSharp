namespace PostoOasis.Core.Models;

public class Produto : IEquatable<Produto>
{
    public int IdProduto { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Categoria { get; set; } = string.Empty;
    public int QuantidadeEstoque { get; set; }
    public int EstoqueMinimo { get; set; } = 5;

    // RN14: Alerta quando estoque mínimo for atingido
    public bool EstoqueAbaixoDoMinimo => QuantidadeEstoque <= EstoqueMinimo;

    public bool Equals(Produto? other)
    {
        if (other is null) return false;
        return IdProduto == other.IdProduto;
    }

    public override bool Equals(object? obj) => Equals(obj as Produto);
    public override int GetHashCode() => IdProduto.GetHashCode();
}
