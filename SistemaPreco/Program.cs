using System;
using System.Globalization; 

public class Program
{
    public static void Main(string[] args)
    {
        decimal valor;
        while (!decimal.TryParse(Console.ReadLine(), out valor))
        {
            Console.WriteLine("Valor inválido");
        }
        
        decimal taxa;
        while (!decimal.TryParse(Console.ReadLine(), out taxa))
        {
            Console.WriteLine("Valor inválido");
        }
        
        decimal total = valor + taxa;
        
        
        Console.WriteLine($"Total confirmado: R$ {total.ToString("F2", CultureInfo.InvariantCulture)}");
    }
}