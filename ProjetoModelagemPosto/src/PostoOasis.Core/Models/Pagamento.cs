namespace PostoOasis.Core.Models;

public enum FormaPagamento
{
    Dinheiro,
    CartaoDebito,
    CartaoCredito,
    Cheque,
    APrazo // Fiado para clientes cadastrados
}

public class Pagamento
{
    public int IdPagamento { get; set; }
    public decimal Valor { get; set; }
    public FormaPagamento Forma { get; set; } = FormaPagamento.Dinheiro;
    public DateTime DataHora { get; set; } = DateTime.Now;
    public string? NumeroCheque { get; set; }
    public string? BancoCheque { get; set; }
    public bool Aprovado { get; set; } = true;
}
