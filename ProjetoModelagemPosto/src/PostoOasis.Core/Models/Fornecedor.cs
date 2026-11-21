using PostoOasis.Core.Services;

namespace PostoOasis.Core.Models;

public class Fornecedor : IEquatable<Fornecedor>
{
    public int IdFornecedor { get; set; }
    public string RazaoSocial { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    // RN15: Posto bandeirado - único fornecedor exclusivo de combustível
    public bool ExclusivoCombustivelBandeirado { get; set; } = false;

    // RN08: Validação matemática do CNPJ
    public bool ValidarCnpj() => ValidadorDocumento.ValidarCnpj(Cnpj);

    public bool Equals(Fornecedor? other)
    {
        if (other is null) return false;
        return string.Equals(Cnpj.Trim(), other.Cnpj.Trim(), StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj) => Equals(obj as Fornecedor);
    public override int GetHashCode() => Cnpj?.Trim().GetHashCode() ?? 0;
}
