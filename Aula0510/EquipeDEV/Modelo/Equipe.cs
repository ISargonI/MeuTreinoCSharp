using System;
using System.Collections.Generic;

public class Equipe
{
    public string Nome { get; }
    public Profissional Lider { get; }

  
    private readonly List<Profissional> _membros;

   
    public IReadOnlyCollection<Profissional> Membros => _membros.AsReadOnly();

    public Equipe(string nome, Profissional lider)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome da equipe é obrigatório.");
            
       
        Lider = lider ?? throw new ArgumentNullException(nameof(lider), "A equipe deve possuir um líder.");
        
       
        _membros = new List<Profissional>();
    }

    public void AdicionarMembro(Profissional membro)
    {
        if (membro == null)
            throw new ArgumentNullException(nameof(membro), "O membro não pode ser nulo.");

        if (_membros.Count >= 6)
            throw new InvalidOperationException("A equipe já atingiu o limite máximo de 6 membros.");

        if (_membros.Contains(membro))
            throw new InvalidOperationException($"O profissional com e-mail '{membro.Email}' já está na equipe.");

        
        if (Lider.Equals(membro))
            throw new InvalidOperationException("O líder já gerencia a equipe e não deve ser adicionado como membro comum.");

        _membros.Add(membro);
    }

    public void RemoverMembro(Profissional membro)
    {
        if (membro == null)
            throw new ArgumentNullException(nameof(membro));

      
        if (!_membros.Contains(membro))
            throw new InvalidOperationException($"O profissional '{membro.Nome}' não faz parte desta equipe.");

        _membros.Remove(membro);
    }
}