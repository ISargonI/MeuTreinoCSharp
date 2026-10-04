using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Gestão do Clube de Leitura ===");

       
        Console.Write("Digite o título do livro para o clube: ");
        string titulo = Console.ReadLine();

        Console.Write("Digite o total de páginas: ");
      
        if (!int.TryParse(Console.ReadLine(), out int totalPaginas))
        {
            Console.WriteLine("Formato incorreto. O total de páginas deve ser um número inteiro.");
            return; 
        }

        try
        {
        
            LivroDiscussao livro1 = new LivroDiscussao(titulo, totalPaginas);
            Console.WriteLine($"\n[Sucesso] Livro '{livro1.Titulo}' cadastrado. Progresso: {livro1.PaginasLidas}/{livro1.TotalPaginas}.");

         
            Console.WriteLine("\n=> Membro leu as primeiras 50 páginas...");
            livro1.AvancarLeitura(50);
            Console.WriteLine($"Novo Progresso: {livro1.PaginasLidas}/{livro1.TotalPaginas} páginas.");
            
          
            Console.WriteLine("\n------------------------------------------------");
            Console.WriteLine("=> Preparando o livro para o próximo mês...");
            LivroDiscussao livro2 = new LivroDiscussao("Viagem à Lua", 150);
            Console.WriteLine($"[Sucesso] Livro '{livro2.Titulo}' cadastrado.");

           
            Console.WriteLine("\n=> Tentando registrar a leitura de 200 páginas (o livro só tem 150)...");
            livro2.AvancarLeitura(200); // Isso fará a classe lançar um ArgumentException
            
           
            Console.WriteLine("Se você está lendo isso, a regra falhou!"); 
        }
      
        catch (ArgumentException ex) 
        {
            Console.WriteLine($"\n[OPERAÇÃO BLOQUEADA PELO OBJETO]: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n[OPERAÇÃO INVÁLIDA]: {ex.Message}");
        }

       
        Console.WriteLine("\n=== Relatório Final ===");
        Console.WriteLine("Operações executadas. O sistema respeitou as regras e impediu estados inconsistentes.");
    }
}
