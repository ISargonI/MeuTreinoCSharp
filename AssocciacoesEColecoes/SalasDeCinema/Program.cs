using System;
using SalasDeCinema.Modelo;

namespace SalasDeCinema
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("--- SISTEMA DE GESTÃO DE CINEMA ---");
 
            Sessao sessao = new Sessao("O Homem Bicentenário", 2);

            Ingresso ing1 = new Ingresso("A1", "Inteira");
            Ingresso ing2 = new Ingresso("A2", "Meia");
            Ingresso ingDuplicado = new Ingresso("A1", "Inteira"); 
            Ingresso ing3 = new Ingresso("A3", "Inteira"); 

            try
            {
                Console.WriteLine("\nTentando vender ingressos válidos...");
                sessao.VenderIngresso(ing1);
                sessao.VenderIngresso(ing2);
                Console.WriteLine("Ingressos A1 e A2 vendidos com sucesso.");

                Console.WriteLine("\nTentando vender o assento A1 novamente...");
                sessao.VenderIngresso(ingDuplicado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO REJEITADO: {ex.Message}");
            }

            try
            {
                Console.WriteLine("\nTentando vender o assento A3...");
                sessao.VenderIngresso(ing3);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO REJEITADO: {ex.Message}");
            }

            // Listagem da Coleção
            Console.WriteLine("\n--- INGRESSOS VENDIDOS NESTA SESSÃO ---");
            foreach (var ingresso in sessao.Ingressos)
            {
                Console.WriteLine($"- Assento: {ingresso.Assento} ({ingresso.Tipo})");
            }

            Console.WriteLine("\nCancelando o ingresso A1...");
            sessao.CancelarIngresso(ing1);

            Console.WriteLine("\n--- INGRESSOS APÓS O CANCELAMENTO ---");
            foreach (var ingresso in sessao.Ingressos)
            {
                Console.WriteLine($"- Assento: {ingresso.Assento} ({ingresso.Tipo})");
            }
        }
    }
}
