using System;

class Program
{
    static void Main()
    {
        Personagem p = new Personagem();

        Console.WriteLine("=== TESTANDO NOME ===");
        p.Nome = "Arthos";
        Console.WriteLine($"Nome atual: '{p.Nome}'");
        p.Nome = "";
        Console.WriteLine($"Nome atual: '{p.Nome}' (Tentou vazio)\n");

        Console.WriteLine("=== TESTANDO NÍVEL ===");
        p.Nivel = 10;
        Console.WriteLine($"Nível atual: {p.Nivel}");
        p.Nivel = 150;
        Console.WriteLine($"Nível atual: {p.Nivel} (Tentou 150)\n");

        Console.WriteLine("=== TESTANDO ENERGIA ===");
        p.Energia = 80;
        Console.WriteLine($"Energia atual: {p.Energia}");
        p.Energia = -20;
        Console.WriteLine($"Energia atual: {p.Energia} (Tentou -20)\n");

        Console.WriteLine("=== TESTANDO VELOCIDADE ===");
        p.Velocidade = 12.5;
        Console.WriteLine($"Velocidade atual: {p.Velocidade}");
        p.Velocidade = 70;
        Console.WriteLine($"Velocidade atual: {p.Velocidade} (Tentou 70)\n");
    }
}