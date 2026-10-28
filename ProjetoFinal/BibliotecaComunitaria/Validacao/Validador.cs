namespace Biblioteca.Negocio.Validacao;

public static class Validador
{
    public static string TextoObrigatorio(string? valor, string campo)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new RegraDeNegocioException($"O campo '{campo}' é obrigatório.");
        return valor.Trim();
    }

    public static Guid Id(Guid valor)
    {
        if (valor == Guid.Empty)
            throw new RegraDeNegocioException("Identificador inválido.");
        return valor;
    }
}