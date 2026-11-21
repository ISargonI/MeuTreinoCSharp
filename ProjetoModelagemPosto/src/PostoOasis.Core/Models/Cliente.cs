using PostoOasis.Core.Services;

namespace PostoOasis.Core.Models;

public class Cliente : IEquatable<Cliente>
{
    public int IdCliente { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CpfCnpj { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.Now;
    public decimal TotalGastoAcumulado { get; set; } = 0;
    public decimal DividaPendente { get; set; } = 0;
    public DateTime? DataVencimentoDivida { get; set; }
    public string? PlacaVeiculoRegistroDivida { get; set; }
    public bool IsEmpresa => CpfCnpj.Length > 11;

    // RN18: Cheque só para clientes com cadastro ativo há mais de 3 meses e sem dívidas
    public bool PodePagarComCheque(DateTime dataReferencia)
    {
        bool maisDeTresMeses = (dataReferencia - DataCadastro).TotalDays >= 90;
        return maisDeTresMeses && DividaPendente == 0;
    }

    // RN20: Inadimplente se tiver dívida após dia 5 do mês seguinte
    public bool EstaInadimplente(DateTime dataReferencia)
    {
        if (DividaPendente <= 0 || !DataVencimentoDivida.HasValue)
            return false;

        return dataReferencia.Date > DataVencimentoDivida.Value.Date;
    }

    // RN10 e RN20: Pode comprar a prazo se cadastrado e não inadimplente
    public bool PodeComprarAPrazo(DateTime dataReferencia)
    {
        return !string.IsNullOrWhiteSpace(CpfCnpj) && !EstaInadimplente(dataReferencia);
    }

    // Conceito POO: Equals e GetHashCode baseados em documento único
    public bool Equals(Cliente? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(CpfCnpj.Trim(), other.CpfCnpj.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as Cliente);

    public override int GetHashCode() => CpfCnpj?.Trim().ToLowerInvariant().GetHashCode() ?? 0;
}
