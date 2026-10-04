using System;

class Program
{
    static void Main()
    {
        try
        {
            Projeto projeto = new Projeto("Sistema de Gestão Escolar");
            Console.WriteLine($"Projeto criado: {projeto.Nome}\n");

            Tarefa t1 = new Tarefa("T-01", "Modelar o banco de dados");
            Tarefa t2 = new Tarefa("T-02", "Criar API de alunos");
            Tarefa t3 = new Tarefa("T-03", "Desenvolver tela de login");

            Console.WriteLine("--- Adicionando tarefas ao projeto ---");
            projeto.AdicionarTarefa(t1);
            projeto.AdicionarTarefa(t2);
            projeto.AdicionarTarefa(t3);
            ListarTarefas(projeto);

            Console.WriteLine("\n--- Alterando situação de uma tarefa (Comportamento da Tarefa) ---");
            t1.Concluir();
            Console.WriteLine($"Tarefa {t1.Codigo} concluída com sucesso.");

            Console.WriteLine("\n--- Tentando remover uma tarefa concluída ---");
            try
            {
                projeto.RemoverTarefa(t1);
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Removendo uma tarefa que ainda pode ser removida ---");
            projeto.RemoverTarefa(t2); // T-02 ainda está pendente
            Console.WriteLine($"Tarefa {t2.Codigo} removida com sucesso.");

            Console.WriteLine("\n--- Tentando adicionar tarefa com código duplicado ---");
            try
            {
                projeto.AdicionarTarefa(new Tarefa("T-01", "Outra atividade qualquer"));
            }
            catch (Exception ex) { Console.WriteLine($"Erro esperado: {ex.Message}"); }

            Console.WriteLine("\n--- Estado final do Projeto ---");
            ListarTarefas(projeto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro inesperado: {ex.Message}");
        }
    }

    static void ListarTarefas(Projeto projeto)
    {
        Console.WriteLine($"Total de tarefas no projeto: {projeto.Tarefas.Count}");
        foreach (var tarefa in projeto.Tarefas)
        {
            Console.WriteLine($"- [{tarefa.Codigo}] {tarefa.Titulo} (Situação: {tarefa.Situacao})");
        }
    }
}
