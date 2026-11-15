namespace Modelo.Produto
{
    public class Produto
    {
    public string Codigo { get; private set; }
    public string Descricao { get; private set; }
    public double PrecoBase { get; private set; }

    public Produto(string codigo, string descricao, double precoBase)
    {
        if (string.IsNullOrWhiteSpace(codigo)) throw new ArgumentException("Código obrigatório.");
        if (string.IsNullOrWhiteSpace(descricao)) throw new ArgumentException("Descrição obrigatória.");
        if (precoBase <= 0) throw new ArgumentOutOfRangeException("Preço base deve ser maior que zero.");

        Codigo = codigo;
        Descricao = descricao;
        PrecoBase = precoBase;
    }

    public void ExibirInformacoes()
    {
        Console.WriteLine($"[{Codigo}] {Descricao} - R$ {PrecoBase:F2}");
    }
    }
}