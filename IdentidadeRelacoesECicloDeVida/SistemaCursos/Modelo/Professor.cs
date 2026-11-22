namespace SistemaCursos.Modelo;

public class Professor
{
    public string Nome { get; }
    public string Especialidade { get; }

    public Professor(string nome, string especialidade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do professor é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(especialidade))
            throw new ArgumentException("Especialidade é obrigatória.", nameof(especialidade));

        Nome = nome.Trim();
        Especialidade = especialidade.Trim();
    }

    public override string ToString() => $"{Nome} ({Especialidade})";
}