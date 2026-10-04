using System;

public class Servico
{
    public string Codigo { get; }
    public string Descricao { get; }
    public decimal ValorBase { get; }

    public Servico(string codigo, string descricao, decimal valorBase)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException("O código do serviço é obrigatório.");
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição é obrigatória.");
        if (valorBase <= 0)
            throw new ArgumentException("O valor base deve ser maior que zero.");

        Codigo = codigo;
        Descricao = descricao;
        ValorBase = valorBase;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Servico outroServico)
        {
            return Codigo.Equals(outroServico.Codigo, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Codigo.ToLowerInvariant().GetHashCode();
    }
}