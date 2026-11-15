namespace SistemaGerenciadoresVeiculos.Modelo
{
    public class Veiculo
{
    public string Placa { get; private set; }
    public string Marca { get; private set; }
    public string Modelo { get; private set; }
    public int AnoFabricacao { get; private set; }
    public double Quilometragem { get; private set; }

    public Veiculo(string placa, string marca, string modelo, int ano, double quilometragem)
    {
        if (string.IsNullOrWhiteSpace(placa) || string.IsNullOrWhiteSpace(marca) || string.IsNullOrWhiteSpace(modelo))
            throw new ArgumentException("Placa, marca e modelo são obrigatórios.");
        
        if (ano < 1900 || ano > DateTime.Now.Year + 1)
            throw new ArgumentOutOfRangeException("Ano de fabricação inválido.");
            
        if (quilometragem < 0)
            throw new ArgumentOutOfRangeException("Quilometragem inicial não pode ser negativa.");

        Placa = placa;
        Marca = marca;
        Modelo = modelo;
        AnoFabricacao = ano;
        Quilometragem = quilometragem;
    }

    public void RegistrarQuilometragem(double km)
    {
        if (km > 0) Quilometragem += km;
    }

    public void ExibirDados()
    {
        Console.WriteLine($"{Marca} {Modelo} ({AnoFabricacao}) - Placa: {Placa} - KM: {Quilometragem}");
    }
}
}