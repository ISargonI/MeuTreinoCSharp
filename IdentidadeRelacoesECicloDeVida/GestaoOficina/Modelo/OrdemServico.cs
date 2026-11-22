namespace GestaoOficina.Modelo;

public class OrdemServico
{
    private readonly List<ExecucaoServico> _execucoes = new();
    private readonly List<Equipamento> _equipamentos = new();

    public string Numero { get; }
    public Cliente Cliente { get; }
    public bool Cancelada { get; private set; }

    public IReadOnlyCollection<ExecucaoServico> Execucoes => _execucoes.AsReadOnly();
    public IReadOnlyCollection<Equipamento> Equipamentos => _equipamentos.AsReadOnly();

    public OrdemServico(string numero, Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("Número da OS é obrigatório.", nameof(numero));

        Numero = numero.Trim();
        Cliente = cliente ?? throw new ArgumentNullException(nameof(cliente));
        Cancelada = false;
    }

    public void AlocarEquipamento(Equipamento equipamento)
    {
        ArgumentNullException.ThrowIfNull(equipamento);

        if (Cancelada)
            throw new InvalidOperationException("Não é possível alocar equipamento em uma OS cancelada.");

        if (!_equipamentos.Contains(equipamento))
            _equipamentos.Add(equipamento);
    }

    public void AdicionarExecucao(Mecanico mecanico, string descricao, decimal valor)
    {
        ArgumentNullException.ThrowIfNull(mecanico);

        if (Cancelada)
            throw new InvalidOperationException("Não é possível adicionar serviços em uma OS cancelada.");

        bool mecanicoJaAlocado = _execucoes.Any(e => e.Mecanico.Equals(mecanico));
        if (mecanicoJaAlocado)
            throw new InvalidOperationException($"Mecânico {mecanico.CodigoRegistro} já está alocado nesta OS.");

        _execucoes.Add(new ExecucaoServico(mecanico, descricao, valor));
    }

    public ExecucaoServico? BuscarExecucaoPorMecanico(Mecanico mecanico)
    {
        ArgumentNullException.ThrowIfNull(mecanico);
        return _execucoes.FirstOrDefault(e => e.Mecanico.Equals(mecanico));
    }

    public void ConcluirExecucao(Mecanico mecanico)
    {
        var execucao = BuscarExecucaoPorMecanico(mecanico)
            ?? throw new InvalidOperationException("Execução de serviço não encontrada para este mecânico.");

        execucao.Concluir();
    }

    public void CancelarExecucao(Mecanico mecanico)
    {
        var execucao = BuscarExecucaoPorMecanico(mecanico)
            ?? throw new InvalidOperationException("Execução de serviço não encontrada para este mecânico.");

        execucao.Cancelar();
    }

    public void CancelarOrdem()
    {
        if (Cancelada)
            return;

        Cancelada = true;

        // Composição: todas as execuções vinculadas são canceladas junto com a OS.
        foreach (var execucao in _execucoes)
        {
            if (execucao.Status == StatusExecucao.Agendada)
                execucao.Cancelar();
        }
    }
}