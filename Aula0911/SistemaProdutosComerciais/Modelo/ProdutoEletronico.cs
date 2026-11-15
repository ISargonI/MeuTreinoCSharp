namespace Modelo.Produto
{

public class ProdutoEletronico : Produto
{
    public string Marca { get; private set; }
    public int GarantiaMeses { get; private set; }

    public ProdutoEletronico(string codigo, string descricao, double precoBase, string marca, int garantia) 
        : base(codigo, descricao, precoBase)
    {
        if (string.IsNullOrWhiteSpace(marca)) throw new ArgumentException("Marca é obrigatória.");
        if (garantia < 0) throw new ArgumentOutOfRangeException("Garantia não pode ser negativa.");

        Marca = marca;
        GarantiaMeses = garantia;
    }

    public void ExibirGarantia() => Console.WriteLine($"Garantia de {GarantiaMeses} meses.");
    }
}