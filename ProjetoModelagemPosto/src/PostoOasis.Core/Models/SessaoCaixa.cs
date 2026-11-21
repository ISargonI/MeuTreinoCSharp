namespace PostoOasis.Core.Models;

public class SessaoCaixa
{
    public int IdSessao { get; set; }
    public DateTime DataAbertura { get; set; } = DateTime.Now;
    public DateTime? DataFechamento { get; set; }
    public decimal SaldoAbertura { get; set; }
    public Funcionario OperadorAbertura { get; set; } = null!;
    public Gerente? GerenteValidador { get; set; } // RN16: Validado por perfil Gerente

    public decimal TotalDinheiro { get; set; }
    public decimal TotalCartaoDebito { get; set; }
    public decimal TotalCartaoCredito { get; set; }
    public decimal TotalCheque { get; set; }
    public decimal TotalAPrazo { get; set; }
    public decimal SaldoTotal => SaldoAbertura + TotalDinheiro + TotalCartaoDebito + TotalCartaoCredito + TotalCheque + TotalAPrazo;

    public bool EstaAberta => !DataFechamento.HasValue;
}
