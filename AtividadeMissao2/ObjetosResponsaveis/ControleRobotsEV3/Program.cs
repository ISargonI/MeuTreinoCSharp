using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Controle de Kits Lego Mindstorms EV3 ===");

        Console.Write("Informe o código/nome do Kit (ex: EV3-Alpha): ");
        string idKit = Console.ReadLine();

        try
        {
            KitLegoEV3 kit1 = new KitLegoEV3(idKit);
            Console.WriteLine($"\n[Sucesso] {kit1.Identificacao} cadastrado. Bateria: {kit1.NivelBateria}%. Status Em Uso: {kit1.EmUso}");

          
            Console.WriteLine("\n=> Iniciando o minicurso... Emprestando o kit.");
            kit1.EmprestarParaMinicurso();
            Console.WriteLine($"Status Em Uso alterado para: {kit1.EmUso}");

           Console.Write("\nFim da atividade. Informe o % de bateria restante para devolver o kit: ");

        int bateria;
  
            while (!int.TryParse(Console.ReadLine(), out bateria))
                {
                    Console.WriteLine("Entrada inválida. Digite apenas números inteiros (ex: 85).");
                    Console.Write("Tente novamente: Informe o % de bateria restante: ");
                }

            kit1.Devolver(bateria);
            Console.WriteLine($"Kit devolvido. Bateria atualizada para {kit1.NivelBateria}%.");

    

          
            Console.WriteLine("\n------------------------------------------------");
            Console.WriteLine("=> Cadastrando um segundo kit de reserva...");
            KitLegoEV3 kit2 = new KitLegoEV3("EV3-Beta");
            
            Console.WriteLine("\n=> Tentando devolver um kit que nem sequer foi emprestado (Operação Inválida)...");
            
            kit2.Devolver(90); 
            
        }
        catch (ArgumentOutOfRangeException ex) 
        {
            Console.WriteLine($"\n[ERRO DE VALOR]: {ex.Message}");
        }
        catch (InvalidOperationException ex) 
        {
            Console.WriteLine($"\n[REGRA BLOQUEADA]: {ex.Message}");
        }
        catch (ArgumentException ex) 
        {
            Console.WriteLine($"\n[ERRO DE CRIAÇÃO]: {ex.Message}");
        }

        Console.WriteLine("\n=== Relatório Final ===");
        Console.WriteLine("As restrições de encapsulamento protegeram o estado dos robôs com sucesso.");
    }
}
