using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Mapeamento de Satélites Noturnos ===");

     
        Console.Write("Informe o nome do satélite (ex: ISS, Starlink): ");
        string nome = Console.ReadLine();

        Console.Write("Informe a posição inicial (azimute de 0 a 360 graus): ");
        double azimute;
        // O laço insiste até que a conversão funcione[cite: 4]
        while (!double.TryParse(Console.ReadLine(), out azimute))
        {
            Console.WriteLine("Erro de formatação. Digite apenas números para os graus.");
            Console.Write("Tente novamente (0 a 360): ");
        }

        try
        {
            RegistroSatelite satelite1 = new RegistroSatelite(nome, azimute);
            Console.WriteLine($"\n[Sucesso] {satelite1.Nome} localizado no azimute {satelite1.Azimute}°. Rastreio iniciado.");

            Console.WriteLine("\n=> Observação em andamento... Adicionando 15 minutos ao registro.");
            satelite1.AdicionarTempoVisibilidade(15);
            Console.WriteLine($"Tempo total de observação: {satelite1.MinutosObservados} minutos.");

            Console.WriteLine("=> O satélite sumiu no horizonte. Encerrando rastreio.");
            satelite1.EncerrarRastreio();
            
         
            Console.WriteLine("\n------------------------------------------------");
            Console.WriteLine("=> Cadastrando um segundo satélite para demonstrar bloqueio de regra...");
            RegistroSatelite satelite2 = new RegistroSatelite("Hubble", 180.5);
            
            Console.WriteLine("=> Encerrando o rastreio do Hubble...");
            satelite2.EncerrarRastreio();

            Console.WriteLine("=> Tentando adicionar tempo a um rastreio que já foi encerrado (Operação Inválida)...");
            
            satelite2.AdicionarTempoVisibilidade(10); 
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine($"\n[ERRO DE COORDENADA]: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"\n[REGRA BLOQUEADA]: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"\n[ERRO DE DADO]: {ex.Message}");
        }

     
        Console.WriteLine("\n=== Relatório Final ===");
        Console.WriteLine("As validações protegeram as coordenadas celestes e a integridade do tempo de observação.");
    }
}
