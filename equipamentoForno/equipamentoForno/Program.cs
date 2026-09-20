using System;

class Program
{
    static void Main()
    {
        Equipamento eqp = new Equipamento();

        Console.WriteLine("=== TESTANDO DESCRIÇÃO ===");

        eqp.Descricao = "Forno Industrial";
        Console.WriteLine($"Descrição atual: '{eqp.Descricao}'");

        eqp.Descricao = "";
        Console.WriteLine($"Descrição atual: '{eqp.Descricao}' (Tentou vazio)");

        eqp.Descricao = "   ";
        Console.WriteLine($"Descrição atual: '{eqp.Descricao}' (Tentou espaços)\n");


        Console.WriteLine("=== TESTANDO TEMPERATURA ===");

        eqp.Temperatura = 25;
        Console.WriteLine($"Temperatura atual: {eqp.Temperatura}");

        eqp.Temperatura = 80;
        Console.WriteLine($"Temperatura atual: {eqp.Temperatura}");

        eqp.Temperatura = 5;
        Console.WriteLine($"Temperatura atual: {eqp.Temperatura} (Tentou 5)");

        eqp.Temperatura = 100;
        Console.WriteLine($"Temperatura atual: {eqp.Temperatura} (Tentou 100)");

        eqp.Temperatura = 60;
        Console.WriteLine($"Temperatura atual: {eqp.Temperatura}");
    }
}
