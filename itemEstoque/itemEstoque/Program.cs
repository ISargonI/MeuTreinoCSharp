using System;

class Program
{
    static void Main()
    {
        ItemEstoque item = new ItemEstoque();

        Console.WriteLine("=== ESTADO INICIAL ===");
        Console.WriteLine($"Nome: {item.Nome}");
        Console.WriteLine($"Quantidade: {item.Quantidade}\n");

        Console.WriteLine("=== ATRIBUINDO VALORES VÁLIDOS ===");
        item.AtualizarNome("Teclado");
        item.AtualizarQuantidade(20);
        Console.WriteLine($"Nome: {item.Nome}");
        Console.WriteLine($"Quantidade: {item.Quantidade}\n");

        Console.WriteLine("=== TESTE DE VALOR INVÁLIDO ===");
        // Descomente UMA linha por vez para ver o sistema barrar a alteração[cite: 3].
        
        // item.AtualizarNome(null);[cite: 3]
        // item.AtualizarNome("");[cite: 3]
        // item.AtualizarNome("   ");[cite: 3]
        // item.AtualizarQuantidade(-5);[cite: 3]
    }
}
