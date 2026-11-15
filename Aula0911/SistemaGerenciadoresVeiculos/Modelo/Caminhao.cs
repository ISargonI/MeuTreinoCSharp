namespace SistemaGerenciadoresVeiculos.Modelo
{
    public class Caminhao : Veiculo
{
    public double CapacidadeCargaKg { get; private set; }
    public int Eixos { get; private set; }

    public Caminhao(string placa, string marca, string modelo, int ano, double km, double capacidadeCarga, int eixos)
        : base(placa, marca, modelo, ano, km)
    {
        if (capacidadeCarga <= 0) throw new ArgumentOutOfRangeException("Capacidade de carga inválida.");
        if (eixos <= 0) throw new ArgumentOutOfRangeException("Quantidade de eixos inválida.");

        CapacidadeCargaKg = capacidadeCarga;
        Eixos = eixos;
    }

    public bool VerificarCapacidade(double pesoCarga) => pesoCarga > 0 && pesoCarga <= CapacidadeCargaKg;
}
}