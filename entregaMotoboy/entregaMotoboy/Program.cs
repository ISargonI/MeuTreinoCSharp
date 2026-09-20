class Program
{
    static void Main(string[] args)
    {
        // Primeira entrega (Curta distância)
        EntregaMotoboy entrega1 = new EntregaMotoboy();
        entrega1.Destinatario = "Restaurante Central";
        entrega1.DistanciaKm = 5.5;
        entrega1.ValorBase = 8.00m;
        entrega1.ValorAdicionalKm = 1.50m;
        entrega1.ExibirDados();

        // Segunda entrega (Longa distância)
        EntregaMotoboy entrega2 = new EntregaMotoboy();
        entrega2.Destinatario = "Condomínio das Árvores";
        entrega2.DistanciaKm = 18.2;
        entrega2.ValorBase = 8.00m;
        entrega2.ValorAdicionalKm = 1.50m;
        entrega2.ExibirDados();

        // Terceira entrega (No limite)
        EntregaMotoboy entrega3 = new EntregaMotoboy();
        entrega3.Destinatario = "Farmácia Saúde";
        entrega3.DistanciaKm = 12.0;
        entrega3.ValorBase = 10.00m;
        entrega3.ValorAdicionalKm = 2.00m;
        entrega3.ExibirDados();
    }
}
