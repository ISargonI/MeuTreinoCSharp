using Biblioteca.Negocio.Validacao;

namespace Biblioteca.Negocio.Modelo;

public class ExemplarLivro
{
    public Guid Id { get; }
    public Livro Livro { get; }
    public string CodigoPatrimonio { get; private set; }
    public bool Disponivel { get; private set; } = true;
    public bool Ativo { get; private set; } = true;

        internal ExemplarLivro(Livro livro, Guid id, string codigoPatrimonio)
    {
        Livro = livro ?? throw new ArgumentNullException(nameof(livro));
        Id = Validador.Id(id);
        CodigoPatrimonio = Validador.TextoObrigatorio(codigoPatrimonio, "Código de patrimônio");
    }

    public void Emprestar()
    {
        ExigirAtivo();
        if (!Disponivel)
            throw new RegraDeNegocioException($"O exemplar '{CodigoPatrimonio}' já está emprestado (indisponível).");
        
        Disponivel = false;
    }

    public void Devolver()
    {
        ExigirAtivo();
        if (Disponivel)
            throw new RegraDeNegocioException($"O exemplar '{CodigoPatrimonio}' já consta como disponível (devolução duplicada).");
        
        Disponivel = true;
    }

    internal void Descartar() => Ativo = false;

    private void ExigirAtivo()
    {
        if (!Ativo)
            throw new RegraDeNegocioException($"O exemplar '{CodigoPatrimonio}' foi descartado do acervo e não pode ser operado.");
    }

    public override bool Equals(object? obj) => obj is ExemplarLivro outro && Id == outro.Id;
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => $"Patrimônio: {CodigoPatrimonio} | Disponível: {Disponivel} | Ativo: {Ativo}";
}