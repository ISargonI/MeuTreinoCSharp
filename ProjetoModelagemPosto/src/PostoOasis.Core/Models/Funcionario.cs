using PostoOasis.Core.Services;

namespace PostoOasis.Core.Models;

public class Funcionario : IEquatable<Funcionario>
{
    public int IdFuncionario { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public string CarteiraTrabalho { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Endereco { get; set; } = string.Empty;
    public decimal Salario { get; set; }
    public DateTime DataAdmissao { get; set; } = DateTime.Now;
    public bool Ativo { get; set; } = true;
    public string Usuario { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;

    // RN04: Validação obrigatória de CPF
    public bool ValidarCpf() => ValidadorDocumento.ValidarCpf(Cpf);

    // RN05: Salário não pode ser inferior ao piso nacional da categoria
    public static decimal ObterPisoNacional(string cargo)
    {
        return cargo.ToLowerInvariant() switch
        {
            "gerente" => 3500.00m,
            "frentista" => 1650.00m,
            "caixa" => 1700.00m,
            "auxiliar administrativo" => 1800.00m,
            _ => 1518.00m // Salário mínimo vigente
        };
    }

    public bool SalarioValidoParaFuncao() => Salario >= ObterPisoNacional(Cargo);

    // RN07: Funcionário só pode solicitar férias após 12 meses de trabalho
    public bool PodeSolicitarFerias(DateTime dataReferencia)
    {
        return (dataReferencia - DataAdmissao).TotalDays >= 365;
    }

    // Conceito POO: Equals e GetHashCode
    public bool Equals(Funcionario? other)
    {
        if (other is null) return false;
        return string.Equals(Cpf.Trim(), other.Cpf.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as Funcionario);
    public override int GetHashCode() => Cpf?.Trim().GetHashCode() ?? 0;
}
