class Program
{
    static void Main(string[] args)
    {
        // Viagem curta (menos de 20 km)
        ViagemAplicativo viagem1 = new ViagemAplicativo();
        viagem1.NomePassageiro = "Lucas";
        viagem1.DistanciaKm = 8.5;
        viagem1.TaxaFixa = 5.00m;
        viagem1.ValorPorKm = 1.20m;
        viagem1.ExibirResumo();

        // Viagem longa (mais de 20 km)
        ViagemAplicativo viagem2 = new ViagemAplicativo();
        viagem2.NomePassageiro = "Fernanda";
        viagem2.DistanciaKm = 25.3;
        viagem2.TaxaFixa = 5.00m;
        viagem2.ValorPorKm = 1.20m;
        viagem2.ExibirResumo();
    }
}
