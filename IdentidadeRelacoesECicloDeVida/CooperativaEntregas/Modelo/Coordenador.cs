namespace CooperativaEntregas.Modelo;

public class Coordenador
{
    public string Nome { get; }
    public string Turno { get; }

    public Coordenador(string nome, string turno)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do coordenador é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(turno))
            throw new ArgumentException("Turno é obrigatório.", nameof(turno));

        Nome = nome.Trim();
        Turno = turno.Trim();
    }

    public override string ToString() => $"{Nome} (Turno: {Turno})";
}