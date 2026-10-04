using System;

public class Livro
{
    public string Titulo { get; }
    public string Isbn { get; }

    public Livro(string titulo, string isbn)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título do livro é obrigatório.");
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("O ISBN do livro é obrigatório.");

        Titulo = titulo;
        Isbn = isbn;
    }

    // A identidade do Livro é definida pelo seu ISBN
    public override bool Equals(object? obj)
    {
        if (obj is Livro outroLivro)
        {
            return Isbn == outroLivro.Isbn;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Isbn.GetHashCode();
    }
}