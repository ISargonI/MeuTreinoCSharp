public class ItemNotaFiscal
{
    public string Produto { get; private set; }

    internal ItemNotaFiscal(string produto)
    {
        Produto = produto;
    }
}