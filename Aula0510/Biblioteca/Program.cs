using System;

class Program
{
    static void Main()
    {
        try
        {
          
            Livro livro1 = new Livro("Fundação", "9788576570179");
            Livro livro2 = new Livro("Eu, Robô", "9788576572005");
            Livro livro3 = new Livro("Viagem à Lua", "9788542217527");
            Livro livro4 = new Livro("O Homem Bicentenário", "9788576573354");

            Leitor leitor = new Leitor("Adriano", "20261004");

            Console.WriteLine("--- Emprestando 3 livros diferentes ---");
            leitor.EmprestarLivro(livro1);
            leitor.EmprestarLivro(livro2);
            leitor.EmprestarLivro(livro3);
            
        
            ListarEmprestimos(leitor);

            Console.WriteLine("\n--- Tentando emprestar livro já presente ---");
            try
            {
                leitor.EmprestarLivro(livro1);
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Tentando ultrapassar o limite de 3 livros ---");
            try
            {
                leitor.EmprestarLivro(livro4);
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Realizando devolução e novo empréstimo ---");
            leitor.DevolverLivro(livro2);
            Console.WriteLine($"Livro '{livro2.Titulo}' devolvido com sucesso.");
            leitor.EmprestarLivro(livro4);
            Console.WriteLine($"Livro '{livro4.Titulo}' emprestado com sucesso após liberar vaga.");
            ListarEmprestimos(leitor);

            Console.WriteLine("\n--- Tentando devolver livro não emprestado ---");
            try
            {
                leitor.DevolverLivro(livro2); 
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
        }
    }

    static void ListarEmprestimos(Leitor leitor)
    {
        Console.WriteLine($"\nLivros emprestados por {leitor.Nome}:");
        foreach (var livro in leitor.LivrosEmprestados)
        {
            Console.WriteLine($"- {livro.Titulo} (ISBN: {livro.Isbn})");
        }
    }
}