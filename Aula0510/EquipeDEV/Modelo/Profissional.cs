using System;

public class Profissional
{
    public string Nome { get; }
    public string Email { get; }
    public string Especialidade { get; }

    public Profissional(string nome, string email, string especialidade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome é obrigatório.");
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O e-mail é obrigatório.");
        if (string.IsNullOrWhiteSpace(especialidade))
            throw new ArgumentException("A especialidade é obrigatória.");

        Nome = nome;
        Email = email;
        Especialidade = especialidade;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Profissional outroProfissional)
        {
            return Email.Equals(outroProfissional.Email, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Email.ToLowerInvariant().GetHashCode();
    }
}