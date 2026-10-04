using System;

class Program
{
    static void Main()
    {
        try
        {
         
            Sessao sessao = new Sessao("Matrix", new DateTime(2026, 10, 10, 20, 0, 0), 3);
            Console.WriteLine($"Sessão: {sessao.Filme} | Horário: {sessao.Horario} | Capacidade: {sessao.CapacidadeMaxima}\n");

           
            Assento a1 = new Assento("A1", "A");
            Assento a2 = new Assento("A2", "A");
            Assento b1 = new Assento("B1", "B");
            Assento c1 = new Assento("C1", "C");

            Console.WriteLine("--- Reservando assentos ---");
            sessao.ReservarAssento(a1);
            sessao.ReservarAssento(a2);
            Console.WriteLine($"Assentos {a1.Codigo} e {a2.Codigo} reservados com sucesso.");

            Console.WriteLine("\n--- Tentando reservar novamente o mesmo assento ---");
            try
            {
                sessao.ReservarAssento(new Assento("A1", "A")); // Mesma identidade
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Tentando ultrapassar a capacidade ---");
            sessao.ReservarAssento(b1); // Preenche a 3ª e última vaga
            try
            {
                sessao.ReservarAssento(c1); // Deve falhar
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Assentos Reservados (Estado Atual) ---");
            ListarAssentos(sessao);

            Console.WriteLine("\n--- Cancelando uma reserva e fazendo uma nova ---");
            sessao.CancelarReserva(a2);
            Console.WriteLine($"Reserva do assento {a2.Codigo} cancelada.");
            sessao.ReservarAssento(c1);
            Console.WriteLine($"Assento {c1.Codigo} reservado após liberação de vaga.");
            ListarAssentos(sessao);

            Console.WriteLine("\n--- Testando o cancelamento de um assento que não está reservado ---");
            try
            {
                sessao.CancelarReserva(a2); // Já foi cancelado
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
        }
    }

    static void ListarAssentos(Sessao sessao)
    {
        Console.WriteLine($"Total reservado: {sessao.AssentosReservados.Count}/{sessao.CapacidadeMaxima}");
        foreach (var assento in sessao.AssentosReservados)
        {
            Console.WriteLine($"- Fileira: {assento.Fileira}, Código: {assento.Codigo}");
        }
    }
}
