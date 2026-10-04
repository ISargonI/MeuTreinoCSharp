using System;
using System.Collections.Generic;

public class Projeto
{
    public string Nome { get; }

    private readonly List<Tarefa> _tarefas;

    public IReadOnlyCollection<Tarefa> Tarefas => _tarefas.AsReadOnly();

    public Projeto(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do projeto é obrigatório.");

        Nome = nome;
    
        _tarefas = new List<Tarefa>();
    }

    public void AdicionarTarefa(Tarefa tarefa)
    {
        if (tarefa == null)
            throw new ArgumentNullException(nameof(tarefa), "A tarefa não pode ser nula.");

        if (_tarefas.Contains(tarefa))
            throw new InvalidOperationException($"Uma tarefa com o código '{tarefa.Codigo}' já pertence a este projeto.");

        _tarefas.Add(tarefa);
    }

    public void RemoverTarefa(Tarefa tarefa)
    {
        if (tarefa == null)
            throw new ArgumentNullException(nameof(tarefa));

        if (!_tarefas.Contains(tarefa))
            throw new InvalidOperationException($"A tarefa '{tarefa.Codigo}' não faz parte deste projeto.");

        if (tarefa.EstaConcluida())
            throw new InvalidOperationException($"A tarefa '{tarefa.Codigo}' está concluída e não pode ser removida do projeto.");

        _tarefas.Remove(tarefa);
    }
}