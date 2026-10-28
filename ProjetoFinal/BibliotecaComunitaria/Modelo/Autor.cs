using Biblioteca.Negocio.Validacao;

namespace Biblioteca.Negocio.Modelo;

public class Autor
{
    public Guid Id { get; }
    public string Nome { get; private set; }

    public Autor(string nome) : this(Guid.NewGuid(), nome) { }

    public Autor(Guid id, string nome)
    {
        Id = Validador.Id(id);
        Nome = Validador.TextoObrigatorio(nome, "Nome do autor");
    }

    public override bool Equals(object? obj) => obj is Autor outro && Id == outro.Id;
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => $"{Nome} (Id: {Id})";
}