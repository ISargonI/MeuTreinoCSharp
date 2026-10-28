using Biblioteca.Negocio.Modelo;
using Biblioteca.Negocio.Validacao;

Console.WriteLine("=== 1. DEMONSTRAÇÃO DE AGREGAÇÃO (Livro <-> Autor) ===");
var asimov = new Autor("Isaac Asimov");
var livroFundacao = new Livro("Fundação");
var livroRobos = new Livro("Eu, Robô");

livroFundacao.VincularAutor(asimov);
livroRobos.VincularAutor(asimov);
Console.WriteLine($"Autores em '{livroFundacao.Titulo}': {livroFundacao.Autores.Count}");

Tentar(() => livroFundacao.VincularAutor(asimov));

livroRobos.DesvincularAutor(asimov);
Console.WriteLine($"Autores em '{livroRobos.Titulo}' após desvincular: {livroRobos.Autores.Count}");
Console.WriteLine($"Autor continua existindo independentemente: {asimov}\n");

Console.WriteLine("=== 2. DEMONSTRAÇÃO DE COMPOSIÇÃO (Livro *-- ExemplarLivro) ===");
var ex1 = livroFundacao.AdicionarExemplar("PAT-001");
var ex2 = livroFundacao.AdicionarExemplar("PAT-002");
Console.WriteLine($"Exemplares ativos em '{livroFundacao.Titulo}': {livroFundacao.Exemplares.Count}");

ex1.Emprestar();
Tentar(() => ex1.Emprestar()); 
Tentar(() => livroFundacao.DesativarLivro()); 

ex1.Devolver();
Tentar(() => ex1.Devolver()); 

Console.WriteLine("\n=== 3. EFEITO DO ENCERRAMENTO DO TODO SOBRE AS PARTES ===");
livroFundacao.DesativarLivro();
Console.WriteLine($"Livro ativo: {livroFundacao.Ativo} | Exemplares na lista: {livroFundacao.Exemplares.Count}");
Console.WriteLine($"Estado do Exemplar 1 após baixa do livro -> Ativo: {ex1.Ativo}");
Console.WriteLine($"Autor Asimov permanece intacto após baixa do livro: {asimov}");

Tentar(() => ex1.Emprestar());

static void Tentar(Action acao)
{
    try
    {
        acao();
        Console.WriteLine("ERRO: A operação deveria ter sido recusada!");
    }
    catch (RegraDeNegocioException ex)
    {
        Console.WriteLine($"Recusado com sucesso: {ex.Message}");
    }
}
