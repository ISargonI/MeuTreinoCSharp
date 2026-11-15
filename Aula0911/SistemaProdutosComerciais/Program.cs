using System;
using Modelo.Produto;

class Program
{
    static void Main()
    {
        var produto = new Produto("001", "Notebook", 3000.00);
        produto.ExibirInformacoes();
        var produtoAlimenticio = new ProdutoAlimenticio("002", "Leite", 5.00, DateTime.Now.AddDays(5), true);
        produtoAlimenticio.ExibirInformacoes();
    }
}
