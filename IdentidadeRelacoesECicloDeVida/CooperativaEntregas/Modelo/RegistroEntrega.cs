namespace CooperativaEntregas.Modelo;

public enum StatusEntrega
{
    Pendente,
    Entregue,
    Cancelada
}

public class RegistroEntrega
{
    public Entregador Entregador { get; }
    public string EnderecoDestino { get; }
    public decimal TaxaEntrega { get; }
    public StatusEntrega Status { get; private set; }

    internal RegistroEntrega(Entregador entregador, string enderecoDestino, decimal taxaEntrega)
    {
        Entregador = entregador ?? throw new ArgumentNullException(nameof(entregador));

        if (string.IsNullOrWhiteSpace(enderecoDestino))
            throw new ArgumentException("Endereço de destino é obrigatório.", nameof(enderecoDestino));
        if (taxaEntrega <= 0)
            throw new ArgumentOutOfRangeException(nameof(taxaEntrega), "Taxa de entrega deve ser positiva.");

        EnderecoDestino = enderecoDestino.Trim();
        TaxaEntrega = taxaEntrega;
        Status = StatusEntrega.Pendente;
    }

    public void MarcarComoEntregue()
    {
        if (Status == StatusEntrega.Cancelada)
            throw new InvalidOperationException("Não é possível concluir uma entrega já cancelada.");

        Status = StatusEntrega.Entregue;
    }

    public void Cancelar()
    {
        Status = StatusEntrega.Cancelada;
    }

    public override string ToString() =>
        $"Destino: {EnderecoDestino} | Entregador: {Entregador.Nome} | Taxa: R$ {TaxaEntrega:F2} | Status: {Status}";
}