using System;

class Program
{
    static void Main()
    {
        try
        {
   
            Profissional lider = new Profissional("Adriano", "adriano@utfpr.edu.br", "Arquiteto C#");
            Equipe equipe = new Equipe("MeuPrimeiroSistema", lider);

            Console.WriteLine($"Equipe: {equipe.Nome} | Líder: {equipe.Lider.Nome} ({equipe.Lider.Especialidade})\n");

         
            Profissional dev1 = new Profissional("Ana", "ana@empresa.com", "Backend .NET");
            Profissional dev2 = new Profissional("Carlos", "carlos@empresa.com", "Frontend");
            equipe.AdicionarMembro(dev1);
            equipe.AdicionarMembro(dev2);

            Console.WriteLine("--- Membros da Equipe ---");
            ListarMembros(equipe);

         
            Console.WriteLine("\n--- Tentando adicionar instância diferente com mesmo e-mail ---");
            Profissional devDuplicado = new Profissional("Ana Silva", "ana@empresa.com", "Fullstack");
            try
            {
                equipe.AdicionarMembro(devDuplicado);
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

       
            Console.WriteLine("\n--- Preenchendo a capacidade máxima ---");
            equipe.AdicionarMembro(new Profissional("João", "joao@empresa.com", "DBA"));
            equipe.AdicionarMembro(new Profissional("Maria", "maria@empresa.com", "QA"));
            equipe.AdicionarMembro(new Profissional("Pedro", "pedro@empresa.com", "DevOps"));
            equipe.AdicionarMembro(new Profissional("Lucas", "lucas@empresa.com", "Backend"));
            ListarMembros(equipe);

            
            Console.WriteLine("\n--- Tentando ultrapassar 6 membros ---");
            try
            {
                equipe.AdicionarMembro(new Profissional("Tiago", "tiago@empresa.com", "UX"));
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

           
            Console.WriteLine("\n--- Removendo o membro 'Carlos' ---");
            equipe.RemoverMembro(dev2);
            ListarMembros(equipe);

            Console.WriteLine("\n--- Tentando remover membro inexistente ---");
            try
            {
                equipe.RemoverMembro(dev2);
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro crítico: {ex.Message}");
        }
    }

    static void ListarMembros(Equipe equipe)
    {
        Console.WriteLine($"Total de membros (sem o líder): {equipe.Membros.Count}");
        foreach (var membro in equipe.Membros)
        {
            Console.WriteLine($"- {membro.Nome} | E-mail: {membro.Email} | Esp: {membro.Especialidade}");
        }
    }
}
