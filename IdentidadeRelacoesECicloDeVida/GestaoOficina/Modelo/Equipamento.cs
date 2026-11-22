namespace GestaoOficina.Modelo;

public class Equipamento
{
    public string Nome { get; }
    public string Patrimonio { get; }

    public Equipamento(string nome, string patrimonio)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do equipamento é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(patrimonio))
            throw new ArgumentException("Patrimônio é obrigatório.", nameof(patrimonio));

        Nome = nome.Trim();
        Patrimonio = patrimonio.Trim();
    }

    public override string ToString() => $"{Nome} (Patrimônio: {Patrimonio})";
}