using System;

class Program
{
    static void Main()
    {
        try
        {
            // Serviços de catálogo
            Servico srv1 = new Servico("SRV-01", "Troca de Óleo", 50.00m);
            Servico srv2 = new Servico("SRV-02", "Alinhamento", 80.00m);
            Servico srv3 = new Servico("SRV-03", "Substituição de Pastilhas", 120.00m);

            // Ordem inicialmente vazia
            OrdemServico ordem = new OrdemServico("OS-2026-1001");
            Console.WriteLine($"Ordem de Serviço criada: {ordem.Numero}\n");

            // Criação de itens (com e sem observação)
            ItemManutencao item1 = new ItemManutencao(srv1, 1, "Óleo sintético 5W40");
            ItemManutencao item2 = new ItemManutencao(srv2, 1); // Sem observação

            Console.WriteLine("--- Adicionando itens à Ordem ---");
            ordem.AdicionarItem(item1);
            ordem.AdicionarItem(item2);
            ListarItens(ordem);

            Console.WriteLine("\n--- Tentando adicionar o mesmo Serviço novamente ---");
            try
            {
                // Criamos um novo item, mas referente ao mesmo serviço (srv1)
                ItemManutencao itemDuplicado = new ItemManutencao(srv1, 2);
                ordem.AdicionarItem(itemDuplicado);
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Tentando criar item com quantidade inválida ---");
            try
            {
                ItemManutencao itemInvalido = new ItemManutencao(srv3, 0);
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Removendo um item ---");
            ordem.RemoverItemPorServico(srv2);
            Console.WriteLine("Serviço 'Alinhamento' removido.");
            ListarItens(ordem);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
        }
    }

    static void ListarItens(OrdemServico ordem)
    {
        Console.WriteLine($"Itens na {ordem.Numero}:");
        decimal total = 0;
        foreach (var item in ordem.Itens)
        {
            decimal subtotal = item.Quantidade * item.ServicoRealizado.ValorBase;
            total += subtotal;
            string obs = item.Observacao != null ? $" [Obs: {item.Observacao}]" : "";
            
            Console.WriteLine($"- {item.Quantidade}x {item.ServicoRealizado.Descricao} | Base: {item.ServicoRealizado.ValorBase} | Subtotal: {subtotal}{obs}");
        }
        Console.WriteLine($"Total da Ordem: {total}");
    }
}