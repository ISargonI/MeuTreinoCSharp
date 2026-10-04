using System;

class Program
{
    static void Main()
    {
        try
        {
        
            ContaUsuario conta = new ContaUsuario("adriano@utfpr.edu.br");
            Console.WriteLine($"Conta criada: {conta.Email}\n");

           
            Dispositivo disp1 = new Dispositivo("MAC-A1B2", "Notebook Dell", "Windows 11");
            Dispositivo disp2 = new Dispositivo("IMEI-9876", "Smartphone Galaxy", "Android");
            Dispositivo disp3 = new Dispositivo("MAC-C3D4", "Desktop Casa", "Linux Ubuntu");

        
            conta.AutorizarDispositivo(disp1);
            conta.AutorizarDispositivo(disp2);
            conta.AutorizarDispositivo(disp3);

            ListarDispositivos(conta);

            Console.WriteLine("\n--- Tentando autorizar outra instância com o mesmo identificador ---");
            Dispositivo dispDuplicado = new Dispositivo("MAC-A1B2", "Notebook Formatado", "Windows 11");
            try
            {
                conta.AutorizarDispositivo(dispDuplicado);
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Tentando ultrapassar o limite de 5 dispositivos ---");
            conta.AutorizarDispositivo(new Dispositivo("ID-04", "Tablet", "iPadOS"));
            conta.AutorizarDispositivo(new Dispositivo("ID-05", "PC Trabalho", "Windows 10"));
            try
            {
                conta.AutorizarDispositivo(new Dispositivo("ID-06", "Smart TV", "Tizen"));
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Revogando um dispositivo existente ---");
            conta.RevogarDispositivo(disp2);
            Console.WriteLine($"Dispositivo '{disp2.Nome}' revogado.");
            ListarDispositivos(conta);

            Console.WriteLine("\n--- Tentando revogar um dispositivo inexistente ---");
            try
            {
                conta.RevogarDispositivo(disp2);
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Código externo acessa apenas para consulta ---");
            
            Console.WriteLine("A propriedade 'DispositivosAutorizados' não expõe métodos como 'Add' ou 'Remove'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
        }
    }

    static void ListarDispositivos(ContaUsuario conta)
    {
        Console.WriteLine($"Quantidade de dispositivos autorizados: {conta.DispositivosAutorizados.Count}");
        foreach (var disp in conta.DispositivosAutorizados)
        {
            Console.WriteLine($"- [{disp.Identificador}] {disp.Nome} ({disp.SistemaOperacional})");
        }
    }
}
