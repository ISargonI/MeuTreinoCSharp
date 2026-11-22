namespace SistemaCursos.Modelo;

public class Estudante
{
    public string Ra { get; }
    public string Nome { get; private set; }

    public Estudante(string ra, string nome)
    {
        if (string.IsNullOrWhiteSpace(ra))
            throw new ArgumentException("RA é obrigatório.", nameof(ra));
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome é obrigatório.", nameof(nome));

        Ra = ra.Trim();
        Nome = nome.Trim();
    }

    public void AtualizarNome(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException("Nome inválido.", nameof(novoNome));

        Nome = novoNome.Trim();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is not Estudante outro)
            return false;

        return string.Equals(Ra, outro.Ra, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Ra);
    }

    public override string ToString() => $"{Nome} (RA: {Ra})";
}