namespace GestaoOficina.Modelo;

public class Mecanico
{
    public string CodigoRegistro { get; }
    public string Nome { get; private set; }

    public Mecanico(string codigoRegistro, string nome)
    {
        if (string.IsNullOrWhiteSpace(codigoRegistro))
            throw new ArgumentException("Código de registro é obrigatório.", nameof(codigoRegistro));
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do mecânico é obrigatório.", nameof(nome));

        CodigoRegistro = codigoRegistro.Trim();
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

        if (obj is not Mecanico outro)
            return false;

        return string.Equals(CodigoRegistro, outro.CodigoRegistro, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(CodigoRegistro);
    }

    public override string ToString() => $"{Nome} [Reg: {CodigoRegistro}]";
}