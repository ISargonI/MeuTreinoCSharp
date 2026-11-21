namespace PostoOasis.Core.Models;

public class CompraMercadoria
{
    public int IdCompra { get; set; }
    public DateTime Data { get; set; } = DateTime.Now;
    public Fornecedor Fornecedor { get; set; } = null!;
    public string NomeItem { get; set; } = string.Empty;
    public decimal Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
    public decimal PrecoTotal => Quantidade * PrecoUnitario;
    public string Categoria { get; set; } = "Conveniência"; // "Conveniência" ou "Combustível"
}

public class RelatorioHorasFuncionario
{
    public string NomeFuncionario { get; set; } = string.Empty;
    public string Cargo { get; set; } = string.Empty;
    public string Matricula { get; set; } = string.Empty;
    public double HorasTrabalhadasMes { get; set; }
    public double HorasExtras { get; set; }
    public int DiasTrabalhados { get; set; }
    public string StatusJornada { get; set; } = "Regular (100% Carga Horária)";
}
