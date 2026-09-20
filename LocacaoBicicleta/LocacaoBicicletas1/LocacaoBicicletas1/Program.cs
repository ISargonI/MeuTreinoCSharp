class Program
{
   static void Main(string[] args)
    {
        // Primeira locação
        LocacaoBicicleta locacao1 = new LocacaoBicicleta();
        locacao1.NomeCliente = "Tiago";
        locacao1.ModeloBicicleta = "Mountain Bike";
        locacao1.QuantidadeHoras = 3;
        locacao1.ValorHora = 12.50m;
        locacao1.ExibirDados();

        // Segunda locação
        LocacaoBicicleta locacao2 = new LocacaoBicicleta();
        locacao2.NomeCliente = "Beatriz";
        locacao2.ModeloBicicleta = "Bicicleta Elétrica";
        locacao2.QuantidadeHoras = 2;
        locacao2.ValorHora = 25.00m;
        locacao2.ExibirDados();
    }
}