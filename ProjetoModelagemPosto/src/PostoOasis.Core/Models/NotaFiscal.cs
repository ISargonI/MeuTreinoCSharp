namespace PostoOasis.Core.Models;

public class NotaFiscal
{
    public int NumeroNF { get; set; }
    public string ChaveAcesso { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime DataEmissao { get; set; } = DateTime.Now;
    public decimal ValorTotal { get; set; }
    public string CpfCnpjDestinatario { get; set; } = string.Empty;
    public string NomeDestinatario { get; set; } = string.Empty;
    public int IdVendaOrigem { get; set; }
}
