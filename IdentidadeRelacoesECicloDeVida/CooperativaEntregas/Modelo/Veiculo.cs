namespace CooperativaEntregas.Modelo;

public class Veiculo
{
    public string Placa { get; }
    public string Modelo { get; }

    public Veiculo(string placa, string modelo)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("Placa é obrigatória.", nameof(placa));
        if (string.IsNullOrWhiteSpace(modelo))
            throw new ArgumentException("Modelo é obrigatório.", nameof(modelo));

        Placa = placa.Trim().ToUpperInvariant();
        Modelo = modelo.Trim();
    }

    public override string ToString() => $"{Modelo} [{Placa}]";
}