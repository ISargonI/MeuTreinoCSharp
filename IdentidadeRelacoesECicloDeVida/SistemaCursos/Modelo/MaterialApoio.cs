namespace SistemaCursos.Modelo;

public class MaterialApoio
{
    public string Titulo { get; }
    public string Tipo { get; }

    public MaterialApoio(string titulo, string tipo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("Título é obrigatório.", nameof(titulo));
        if (string.IsNullOrWhiteSpace(tipo))
            throw new ArgumentException("Tipo é obrigatório.", nameof(tipo));

        Titulo = titulo.Trim();
        Tipo = tipo.Trim();
    }

    public override string ToString() => $"{Titulo} [{Tipo}]";
}