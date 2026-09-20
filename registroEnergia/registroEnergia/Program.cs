class Program
{
    static void Main(string[] args)
    {
        // Primeiro registro (Consumo normal)
        RegistroEnergia registro1 = new RegistroEnergia();
        registro1.NomeResponsavel = "Ana";
        registro1.MesReferencia = "Setembro";
        registro1.ConsumoKwh = 180;
        registro1.ValorKwh = 0.95m;
        registro1.ExibirDados();

        // Segundo registro (Consumo alto)
        RegistroEnergia registro2 = new RegistroEnergia();
        registro2.NomeResponsavel = "Marcos";
        registro2.MesReferencia = "Setembro";
        registro2.ConsumoKwh = 310;
        registro2.ValorKwh = 0.95m;
        registro2.ExibirDados();

        // Terceiro registro (Consumo alto)
        RegistroEnergia registro3 = new RegistroEnergia();
        registro3.NomeResponsavel = "Juliana";
        registro3.MesReferencia = "Setembro";
        registro3.ConsumoKwh = 260;
        registro3.ValorKwh = 0.95m;
        registro3.ExibirDados();
    }
}
