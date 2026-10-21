using System.Collections.Generic;

public class Playlist
{
    public string Nome { get; private set; }
    private readonly List<Musica> _musicas = new List<Musica>();

    public Playlist(string nome)
    {
        Nome = nome;
    }

    public void AdicionarMusica(Musica musica)
    {
        _musicas.Add(musica);
    }
}