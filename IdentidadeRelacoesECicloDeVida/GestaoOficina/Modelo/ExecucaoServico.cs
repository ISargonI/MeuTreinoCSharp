namespace GestaoOficina.Modelo;

public enum StatusExecucao
{
    Agendada,
    Concluida,
    Cancelada
}

public class ExecucaoServico
{
    public Mecanico Mecanico { get; }
    public string Descricao { get; }
    public decimal Valor { get; }
    public StatusExecucao Status { get; private set; }

    internal ExecucaoServico(Mecanico mecanico, string descricao, decimal valor)
    {
        Mecanico = mecanico ?? throw new ArgumentNullException(nameof(mecanico));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("Descrição do serviço é obrigatória.", nameof(descricao));
        if (valor <= 0)
            throw new ArgumentOutOfRangeException(nameof(valor), "Valor deve ser maior que zero.");

        Descricao = descricao.Trim();
        Valor = valor;
        Status = StatusExecucao.Agendada;
    }

    public void Concluir()
    {
        if (Status == StatusExecucao.Cancelada)
            throw new InvalidOperationException("Não é possível concluir uma execução cancelada.");

        Status = StatusExecucao.Concluida;
    }

    public void Cancelar()
    {
        Status = StatusExecucao.Cancelada;
    }

    public override string ToString() =>
        $"{Descricao} | Mecânico: {Mecanico} | Valor: R$ {Valor:F2} | Status: {Status}";
}