namespace CooperativaEntregas.Modelo;

public class Entregador
{
    public Guid Id { get; }
    public string Nome { get; private set; }
    public string Documento { get; private set; }

    public Entregador(string nome, string documento, Guid? id = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome do entregador é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(documento))
            throw new ArgumentException("Documento é obrigatório.", nameof(documento));

        Id = id ?? Guid.NewGuid();
        Nome = nome.Trim();
        Documento = documento.Trim();
    }

    public void AtualizarDocumento(string novoDocumento)
    {
        if (string.IsNullOrWhiteSpace(novoDocumento))
            throw new ArgumentException("Documento inválido.", nameof(novoDocumento));

        Documento = novoDocumento.Trim();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
            return true;

        if (obj is not Entregador outro)
            return false;

        return Id.Equals(outro.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public override string ToString() => $"{Nome} (Doc: {Documento} | Id: {Id.ToString()[..8]})";
}