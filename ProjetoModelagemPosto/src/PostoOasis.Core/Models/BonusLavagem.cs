namespace PostoOasis.Core.Models;

public class BonusLavagem
{
    public int IdBonus { get; set; }
    public string CodigoVoucher { get; set; } = Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
    public Cliente Cliente { get; set; } = null!;
    public decimal ValorGastoOrigem { get; set; }
    public DateTime DataEmissao { get; set; } = DateTime.Now;
    
    // RN03: Validade de 30 dias a contar da data de emissão
    public DateTime DataValidade => DataEmissao.AddDays(30);
    public bool Utilizado { get; set; } = false;

    // RN03: Expira automaticamente no sistema após 30 dias
    public bool EstaValido(DateTime dataAtual) => !Utilizado && dataAtual.Date <= DataValidade.Date;

    public int DiasRestantes(DateTime dataAtual) => Math.Max(0, (int)(DataValidade.Date - dataAtual.Date).TotalDays);
}
