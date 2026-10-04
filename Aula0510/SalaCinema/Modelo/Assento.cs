using System;

public class Assento
{
    public string Codigo { get; }
    public string Fileira { get; }

    public Assento(string codigo, string fileira)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException("O código do assento é obrigatório.");
        if (string.IsNullOrWhiteSpace(fileira))
            throw new ArgumentException("A fileira do assento é obrigatória.");

        Codigo = codigo;
        Fileira = fileira;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Assento outroAssento)
        {
            return Codigo.Equals(outroAssento.Codigo, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Codigo.ToLowerInvariant().GetHashCode();
    }
}