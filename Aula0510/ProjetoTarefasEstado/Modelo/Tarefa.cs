using System;

public enum SituacaoTarefa
{
    Pendente,
    EmAndamento,
    Concluida
}

public class Tarefa
{
    public string Codigo { get; }
    public string Titulo { get; }
    public SituacaoTarefa Situacao { get; private set; }

    public Tarefa(string codigo, string titulo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException("O código da tarefa é obrigatório.");
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título da tarefa é obrigatório.");

        Codigo = codigo;
        Titulo = titulo;
     
        Situacao = SituacaoTarefa.Pendente;
    }

    public void Iniciar()
    {
        if (Situacao == SituacaoTarefa.Concluida)
            throw new InvalidOperationException("Uma tarefa concluída não pode ser reiniciada.");
            
        Situacao = SituacaoTarefa.EmAndamento;
    }

    public void Concluir()
    {
        Situacao = SituacaoTarefa.Concluida;
    }

    public bool EstaConcluida()
    {
        return Situacao == SituacaoTarefa.Concluida;
    }

  
    public override bool Equals(object? obj)
    {
        if (obj is Tarefa outraTarefa)
        {
            return Codigo.Equals(outraTarefa.Codigo, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Codigo.ToLowerInvariant().GetHashCode();
    }
}