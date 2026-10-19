using System;
using Biblioteca.Modelo;

namespace Biblioteca
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("--- SISTEMA DE BIBLIOTECA ---");
            
            Leitor leitor = new Leitor("Adriano");

            Livro livro1 = new Livro("978-85-7608-267-5", "Código Limpo");
            Livro livro2 = new Livro("978-85-7522-724-4", "Arquitetura Limpa");
            Livro livro3 = new Livro("978-0-13-235088-4", "O Mítico Homem-Mês");
            Livro livro4 = new Livro("978-85-7302-125-7", "Viagem à Lua"); 
    

            try
            {
                Console.WriteLine($"\nRequisitando livros para {leitor.Nome}...");
                leitor.RequisitarLivro(livro1);
                leitor.RequisitarLivro(livro2);
                Console.WriteLine("Livros requisitados com sucesso.");

  
                Console.WriteLine($"\nTentando requisitar o livro '{livro1.Titulo}' novamente...");
                leitor.RequisitarLivro(livro1); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO REJEITADO: {ex.Message}");
            }

            try
            {

                leitor.RequisitarLivro(livro3);

                Console.WriteLine($"\nTentando requisitar o 4º livro ('{livro4.Titulo}')...");
                leitor.RequisitarLivro(livro4);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO REJEITADO: {ex.Message}");
            }

            Console.WriteLine($"\n--- LIVROS NA POSSE DE {leitor.Nome.ToUpper()} ---");
            if (leitor.Emprestimos.Count == 0)
            {
                Console.WriteLine("Nenhum livro requisitado.");
            }
            else
            {
                foreach (var livro in leitor.Emprestimos)
                {
                    Console.WriteLine($"- ISBN: {livro.ISBN} | Título: {livro.Titulo}");
                }
            }

            Console.WriteLine($"\nDevolvendo o livro '{livro2.Titulo}'...");
            leitor.DevolverLivro(livro2);

            Console.WriteLine($"\n--- ATUALIZAÇÃO DOS EMPRÉSTIMOS ---");
            foreach (var livro in leitor.Emprestimos)
            {
                Console.WriteLine($"- ISBN: {livro.ISBN} | Título: {livro.Titulo}");
            }
        }
    }
}
