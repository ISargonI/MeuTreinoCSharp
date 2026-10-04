public class LivroDiscussao
{
  
    public string Titulo { get; }
    public int PaginasLidas { get; private set; }
    public int TotalPaginas { get; }
    public bool DiscussaoConcluida { get; private set; }

   
    public LivroDiscussao(string titulo, int totalPaginas)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título do livro não pode ser vazio.");
        if (totalPaginas <= 0)
            throw new ArgumentException("O total de páginas deve ser maior que zero.");

        Titulo = titulo;
        TotalPaginas = totalPaginas;
        PaginasLidas = 0;
        DiscussaoConcluida = false;
    }

 
    public void AvancarLeitura(int paginas)
    {
        if (paginas <= 0)
            throw new ArgumentException("A quantidade de páginas lidas deve ser positiva.");
        if (PaginasLidas + paginas > TotalPaginas)
            throw new ArgumentException("A leitura não pode ultrapassar o total de páginas da obra.");

        PaginasLidas += paginas;
    }

  
    public void ConcluirDiscussao()
    {
        if (PaginasLidas < TotalPaginas)
            throw new InvalidOperationException("O livro precisa ser lido integralmente antes da discussão.");

        DiscussaoConcluida = true;
    }
}