using System;
using System.Collections.Generic;

public class Leitor
{
    public string Nome { get; }
    public string Matricula { get; }


    private readonly List<Livro> _livrosEmprestados;

  
    public IReadOnlyCollection<Livro> LivrosEmprestados => _livrosEmprestados.AsReadOnly();

    public Leitor(string nome, string matricula)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do leitor é obrigatório.");
        if (string.IsNullOrWhiteSpace(matricula))
            throw new ArgumentException("A matrícula do leitor é obrigatória.");

        Nome = nome;
        Matricula = matricula;
      
        _livrosEmprestados = new List<Livro>();
    }

    public void EmprestarLivro(Livro livro)
    {
        if (livro == null)
            throw new ArgumentNullException(nameof(livro), "O livro não pode ser nulo.");

        if (_livrosEmprestados.Count >= 3)
            throw new InvalidOperationException("O leitor já possui o limite máximo de 3 livros emprestados simultaneamente.");

        if (_livrosEmprestados.Contains(livro))
            throw new InvalidOperationException($"O livro '{livro.Titulo}' já está emprestado para este leitor.");

        _livrosEmprestados.Add(livro);
    }

    public void DevolverLivro(Livro livro)
    {
        if (livro == null)
            throw new ArgumentNullException(nameof(livro), "O livro não pode ser nulo.");

        if (!_livrosEmprestados.Contains(livro))
            throw new InvalidOperationException($"O livro '{livro.Titulo}' não consta nos empréstimos deste leitor.");

        _livrosEmprestados.Remove(livro);
    }
}