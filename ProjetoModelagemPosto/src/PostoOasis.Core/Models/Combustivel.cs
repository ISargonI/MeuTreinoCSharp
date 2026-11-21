namespace PostoOasis.Core.Models;

public enum TipoCombustivel
{
    Gasolina,
    Etanol,
    Diesel
}

public class Combustivel
{
    public int IdCombustivel { get; set; }
    public TipoCombustivel Tipo { get; set; }
    public string Nome => Tipo.ToString();
    public decimal PrecoLitro { get; set; }
    public double CapacidadeTotalLitros { get; set; }
    public double NivelAtualLitros { get; set; }

    public double PercentualNivel => CapacidadeTotalLitros > 0 ? (NivelAtualLitros / CapacidadeTotalLitros) * 100.0 : 0.0;

    // RN12: Tanque não pode ficar abaixo de 4% (bloqueia bomba para não puxar sujeira)
    public bool EstaNivelCriticoBloqueio => PercentualNivel <= 4.0;

    // RN13: Alerta ao atingir 15% para o gerente pedir mais combustível
    public bool PrecisaAlertaCompra => PercentualNivel <= 15.0;

    public void ConsumirLitros(double litros)
    {
        NivelAtualLitros = Math.Max(0, NivelAtualLitros - litros);
    }

    public void Reabastecer(double litros)
    {
        NivelAtualLitros = Math.Min(CapacidadeTotalLitros, NivelAtualLitros + litros);
    }
}
