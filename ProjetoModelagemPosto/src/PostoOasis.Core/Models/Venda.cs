namespace PostoOasis.Core.Models;

public enum StatusVenda
{
    Concluida,
    Cancelada,
    FugaNaoPago // Caloteiro (RN09)
}

public class Venda
{
    public int IdVenda { get; set; }
    public DateTime DataHora { get; set; } = DateTime.Now;
    public List<ItemVenda> ItensProdutos { get; set; } = new();
    public ItemCombustivel? ItemCombustivel { get; set; }
    public Pagamento? Pagamento { get; set; }
    public Cliente? Cliente { get; set; }
    public Funcionario Operador { get; set; } = null!; // Operador de Caixa
    public Funcionario? Frentista { get; set; } // Frentista responsável pelo abastecimento
    public StatusVenda Status { get; set; } = StatusVenda.Concluida;
    public DateTime? DataCancelamento { get; set; }
    public string? MotivoCancelamento { get; set; }
    public NotaFiscal? NotaFiscalEmitida { get; set; }
    public BonusLavagem? BonusLavagemGerado { get; set; }

    public decimal ValorTotal
    {
        get
        {
            decimal totalProdutos = ItensProdutos.Sum(i => i.Subtotal);
            decimal totalCombustivel = ItemCombustivel?.Subtotal ?? 0;
            return totalProdutos + totalCombustivel;
        }
    }

    // Propriedades amigáveis para exibição nas telas e auditoria analítica do Gerente (RF20)
    public string NomeOperadorCaixa => Operador != null ? $"{Operador.Nome} ({Operador.Cargo})" : "Caixa Geral";
    public string NomeFrentista => Frentista != null ? $"{Frentista.Nome} (Frentista)" : "Wilson Silva (Frentista)";
    public string NomeCliente => Cliente != null ? $"{Cliente.Nome} ({Cliente.CpfCnpj})" : "Consumidor Final";
    public string DescricaoCombustivel => ItemCombustivel != null 
        ? $"{ItemCombustivel.Bomba?.Identificador ?? "Bomba"} - {ItemCombustivel.Bomba?.CombustivelAssociado?.Nome ?? "Combustível"} ({ItemCombustivel.Litros:F2} L a R$ {ItemCombustivel.PrecoLitro:F2}/L)" 
        : "N/A";
    public string DescricaoItensLoja => ItensProdutos.Count > 0 
        ? string.Join(", ", ItensProdutos.Select(i => $"{i.Quantidade}x {i.Produto?.Nome ?? "Item"}"))
        : "N/A";
    public string FormaPagamentoTexto => Pagamento != null ? Pagamento.Forma.ToString() : "Dinheiro";

    // RN17: Cancelamento ou estorno só pode ser realizado no prazo máximo de 30 minutos
    public bool PodeCancelar(DateTime agora)
    {
        if (Status != StatusVenda.Concluida) return false;
        return (agora - DataHora).TotalMinutes <= 30.0;
    }
}
