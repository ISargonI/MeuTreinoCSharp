class Program
{
    static void Main(string[] args)
    {
        // Primeiro pedido
        PedidoImpressao pedido1 = new PedidoImpressao();
        pedido1.NomeCliente = "Escritório de Advocacia";
        pedido1.QuantidadePaginas = 50;
        pedido1.NumeroCopias = 3;
        pedido1.PrecoPagina = 0.15m;
        pedido1.ExibirResumo();

        // Segundo pedido
        PedidoImpressao pedido2 = new PedidoImpressao();
        pedido2.NomeCliente = "Escola Municipal";
        pedido2.QuantidadePaginas = 2;
        pedido2.NumeroCopias = 100;
        pedido2.PrecoPagina = 0.10m;
        pedido2.ExibirResumo();
    }
}
