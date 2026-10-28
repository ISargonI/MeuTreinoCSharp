using System;
using SistemaComercial.ConsoleApp; 

class Program
{
    static void Main(string[] args)
    {
        var controlador = new ProdutoController();

        if (args.Length > 0)
        {
            ExecutarModoCLI(args, controlador);
        }
        else
        {
            controlador.IniciarMenuInterativo();
        }
    }

    private static void ExecutarModoCLI(string[] args, ProdutoController ctrl)
    {
        string comando = args[0].ToLower();

        try
        {
            if (comando == "cadastrar" && args.Length == 4)
            {
                string nome = args[1];
                decimal preco = decimal.Parse(args[2]);
                int estoque = int.Parse(args[3]);
                ctrl.Cadastrar(nome, preco, estoque);
            }
            else if (comando == "listar")
            {
                ctrl.Listar();
            }
            else
            {
                Console.WriteLine("Comando CLI inválido ou argumentos insuficientes.");
                Console.WriteLine("Uso: dotnet run -- cadastrar <nome> <preco> <estoque>");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro CLI: {ex.Message}");
        }
    }
}