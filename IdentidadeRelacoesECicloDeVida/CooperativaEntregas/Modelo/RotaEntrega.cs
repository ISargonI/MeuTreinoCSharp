namespace CooperativaEntregas.Modelo;

public class RotaEntrega
{
    private readonly List<RegistroEntrega> _registros = new();
    private readonly List<Veiculo> _veiculos = new();

    public string CodigoRota { get; }
    public Coordenador Coordenador { get; }
    public bool Encerrada { get; private set; }

    public IReadOnlyCollection<RegistroEntrega> Registros => _registros.AsReadOnly();
    public IReadOnlyCollection<Veiculo> Veiculos => _veiculos.AsReadOnly();

    public RotaEntrega(string codigoRota, Coordenador coordenador)
    {
        if (string.IsNullOrWhiteSpace(codigoRota))
            throw new ArgumentException("Código da rota é obrigatório.", nameof(codigoRota));

        CodigoRota = codigoRota.Trim();
        Coordenador = coordenador ?? throw new ArgumentNullException(nameof(coordenador));
        Encerrada = false;
    }

    public void VincularVeiculo(Veiculo veiculo)
    {
        ArgumentNullException.ThrowIfNull(veiculo);

        if (Encerrada)
            throw new InvalidOperationException("Não é possível vincular veículo a uma rota encerrada.");

        if (!_veiculos.Contains(veiculo))
            _veiculos.Add(veiculo);
    }

    public void RegistrarEntrega(Entregador entregador, string enderecoDestino, decimal taxaEntrega)
    {
        ArgumentNullException.ThrowIfNull(entregador);

        if (Encerrada)
            throw new InvalidOperationException("Não é possível registrar entregas em uma rota encerrada.");

        // Regra de duplicidade separada da identidade (Guid):
        // Impede o mesmo entregador (por Id ou por Documento) de ter duas entregas ativas na mesma rota.
        bool duplicado = _registros.Any(r =>
            r.Entregador.Equals(entregador) ||
            string.Equals(r.Entregador.Documento, entregador.Documento, StringComparison.OrdinalIgnoreCase));

        if (duplicado)
            throw new InvalidOperationException(
                $"Entregador com documento {entregador.Documento} já possui entrega registrada nesta rota.");

        _registros.Add(new RegistroEntrega(entregador, enderecoDestino, taxaEntrega));
    }

    public RegistroEntrega? BuscarRegistroPorEntregador(Entregador entregador)
    {
        ArgumentNullException.ThrowIfNull(entregador);
        return _registros.FirstOrDefault(r => r.Entregador.Equals(entregador));
    }

    public void ConfirmarEntrega(Entregador entregador)
    {
        var registro = BuscarRegistroPorEntregador(entregador)
            ?? throw new InvalidOperationException("Registro de entrega não encontrado para este entregador.");

        registro.MarcarComoEntregue();
    }

    public void CancelarEntrega(Entregador entregador)
    {
        var registro = BuscarRegistroPorEntregador(entregador)
            ?? throw new InvalidOperationException("Registro de entrega não encontrado para este entregador.");

        registro.Cancelar();
    }

    public void EncerrarRota()
    {
        if (Encerrada)
            return;

        Encerrada = true;

        // Composição: entregas pendentes são canceladas ao encerrar a rota.
        foreach (var registro in _registros)
        {
            if (registro.Status == StatusEntrega.Pendente)
                registro.Cancelar();
        }
    }
}