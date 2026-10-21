using System.Collections.Generic;

public class NotaFiscal
{
    public int Numero { get; private set; }
    
    private readonly List<ItemNotaFiscal> _itens = new List<ItemNotaFiscal>();

    public IReadOnlyCollection<ItemNotaFiscal> Itens => _itens.AsReadOnly();

    public NotaFiscal(int numero)
    {
        Numero = numero;
    }

    public void AdicionarItem(string produto)
    {
        var item = new ItemNotaFiscal(produto);
        _itens.Add(item);
    }
}