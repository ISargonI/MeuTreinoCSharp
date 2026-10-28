using Biblioteca.Negocio.Validacao;

namespace Biblioteca.Negocio.Modelo;

public class Livro
{
    private readonly List<Autor> _autores = new();
    private readonly List<ExemplarLivro> _exemplares = new();

    public Guid Id { get; }
    public string Titulo { get; private set; }
    public bool Ativo { get; private set; } = true;

    public IReadOnlyList<Autor> Autores => _autores.AsReadOnly();
    public IReadOnlyList<ExemplarLivro> Exemplares => _exemplares.AsReadOnly();

    public Livro(string titulo) : this(Guid.NewGuid(), titulo) { }

    public Livro(Guid id, string titulo)
    {
        Id = Validador.Id(id);
        Titulo = Validador.TextoObrigatorio(titulo, "Título do livro");
    }

    public void VincularAutor(Autor autor)
    {
        ExigirAtivo();
        if (autor is null)
            throw new RegraDeNegocioException("Autor inválido.");
        if (_autores.Contains(autor))
            throw new RegraDeNegocioException($"O autor '{autor.Nome}' já está vinculado a este livro.");

        _autores.Add(autor);
    }

    public void DesvincularAutor(Autor autor)
    {
        ExigirAtivo();
        if (autor is null || !_autores.Remove(autor))
            throw new RegraDeNegocioException("Autor não está vinculado a este livro.");
    }

    public ExemplarLivro AdicionarExemplar(string codigoPatrimonio)
    {
        ExigirAtivo();
        string codigoValido = Validador.TextoObrigatorio(codigoPatrimonio, "Código de patrimônio");
        
        if (_exemplares.Any(e => string.Equals(e.CodigoPatrimonio, codigoValido, StringComparison.OrdinalIgnoreCase)))
            throw new RegraDeNegocioException($"Já existe um exemplar com o patrimônio '{codigoValido}' neste livro.");

        var exemplar = new ExemplarLivro(this, Guid.NewGuid(), codigoValido);
        _exemplares.Add(exemplar);
        return exemplar;
    }

    public void RemoverExemplar(Guid exemplarId)
    {
        ExigirAtivo();
        ExemplarLivro exemplar = ObterExemplar(exemplarId);
        if (!exemplar.Disponivel)
            throw new RegraDeNegocioException("Não é possível remover um exemplar que está emprestado.");

        _exemplares.Remove(exemplar);
        exemplar.Descartar();
    }

    public ExemplarLivro ObterExemplar(Guid exemplarId)
    {
        var exemplar = _exemplares.FirstOrDefault(e => e.Id == exemplarId);
        if (exemplar is null)
            throw new RegraDeNegocioException("Exemplar não encontrado neste livro.");
        return exemplar;
    }

      public void DesativarLivro()
    {
        ExigirAtivo();
        if (_exemplares.Any(e => !e.Disponivel))
            throw new RegraDeNegocioException("Impossível desativar o livro: há exemplares com empréstimo ativo.");

        foreach (var exemplar in _exemplares)
            exemplar.Descartar();

        _exemplares.Clear();
        _autores.Clear();
        Ativo = false;
    }

    private void ExigirAtivo()
    {
        if (!Ativo)
            throw new RegraDeNegocioException($"O livro '{Titulo}' foi desativado do acervo.");
    }
}