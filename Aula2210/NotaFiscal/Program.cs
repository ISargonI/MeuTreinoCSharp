using System;

var nota = new NotaFiscal(1001);


nota.AdicionarItem("Teclado Mecânico"); 
nota.AdicionarItem("Rato Sem Fios");
nota.AdicionarItem("Monitor 24 polegadas");

Console.WriteLine($"--- NOTA FISCAL Nº {nota.Numero} ---");
Console.WriteLine("Itens registados:");

foreach (var item in nota.Itens)
{
    Console.WriteLine($"- {item.Produto}");
}