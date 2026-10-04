using System;

public class Dispositivo
{
    public string Identificador { get; }
    public string Nome { get; }
    public string SistemaOperacional { get; }

    public Dispositivo(string identificador, string nome, string sistemaOperacional)
    {
        if (string.IsNullOrWhiteSpace(identificador))
            throw new ArgumentException("O identificador do dispositivo é obrigatório.");
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do dispositivo é obrigatório.");
        if (string.IsNullOrWhiteSpace(sistemaOperacional))
            throw new ArgumentException("O sistema operacional é obrigatório.");

        Identificador = identificador;
        Nome = nome;
        SistemaOperacional = sistemaOperacional;
    }


    public override bool Equals(object? obj)
    {
        if (obj is Dispositivo outroDispositivo)
        {
            return Identificador.Equals(outroDispositivo.Identificador, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Identificador.ToLowerInvariant().GetHashCode();
    }
}