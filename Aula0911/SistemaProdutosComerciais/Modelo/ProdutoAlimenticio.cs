namespace Modelo.Produto
{

public class ProdutoAlimenticio : Produto
{
    public DateTime DataValidade { get; private set; }
    public bool PrecisaRefrigeracao { get; private set; }

    public ProdutoAlimenticio(string codigo, string descricao, double precoBase, DateTime validade, bool refrigeracao) 
        : base(codigo, descricao, precoBase)
    {
        DataValidade = validade;
        PrecisaRefrigeracao = refrigeracao;
    }

    public bool EstaVencido() => DateTime.Now > DataValidade;
}
}