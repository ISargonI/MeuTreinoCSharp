namespace GestaoOficina.Modelo;

public class Cliente
{
    public string Nome { get; }
    public string Telefone { get; }

    public Cliente(string nome, string telefone)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do cliente é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(telefone))
            throw new ArgumentException("Telefone é obrigatório.", nameof(telefone));

        Nome = nome.Trim();
        Telefone = telefone.Trim();
    }

    public override string ToString() => $"{Nome} (Tel: {Telefone})";
}