class Program
{
    static void Main()
    {
        var musica = new Musica("Luar do Sertão"); 
        
        var playlist = new Playlist("Clássicos Sertanejo");
        
        playlist.AdicionarMusica(musica); 

        Console.WriteLine($"Música: {musica.Titulo}");
        Console.WriteLine($"Playlist: {playlist.Nome}");
    }
}
