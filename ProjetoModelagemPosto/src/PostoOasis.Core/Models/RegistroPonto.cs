namespace PostoOasis.Core.Models;

public enum TipoMarcacaoPonto
{
    Entrada,
    SaidaIntervalo,
    RetornoIntervalo,
    Saida
}

public class RegistroPonto
{
    public int IdRegistro { get; set; }
    public Funcionario Funcionario { get; set; } = null!;
    public DateTime DataHora { get; set; } = DateTime.Now;
    public TipoMarcacaoPonto Tipo { get; set; }
}
