namespace SistemaCursos.Modelo;

public enum StatusMatricula
{
    Pendente,
    Ativa,
    Cancelada
}

public class Matricula
{
    public Estudante Estudante { get; }
    public DateTime DataMatricula { get; }
    public decimal? NotaFinal { get; private set; }
    public StatusMatricula Status { get; private set; }

    internal Matricula(Estudante estudante)
    {
        Estudante = estudante ?? throw new ArgumentNullException(nameof(estudante));
        DataMatricula = DateTime.Now;
        Status = StatusMatricula.Pendente;
    }

    public void Ativar()
    {
        if (Status == StatusMatricula.Cancelada)
            throw new InvalidOperationException("Não é possível ativar uma matrícula cancelada.");

        Status = StatusMatricula.Ativa;
    }

    public void Cancelar()
    {
        Status = StatusMatricula.Cancelada;
    }

    public void RegistrarNota(decimal nota)
    {
        if (Status != StatusMatricula.Ativa)
            throw new InvalidOperationException("Apenas matrículas ativas podem receber nota.");
        if (nota < 0 || nota > 10)
            throw new ArgumentOutOfRangeException(nameof(nota), "A nota deve estar entre 0 e 10.");

        NotaFinal = nota;
    }

    public override string ToString()
    {
        string notaTexto = NotaFinal.HasValue ? NotaFinal.Value.ToString("F1") : "Sem nota";
        return $"{Estudante} | Data: {DataMatricula:dd/MM/yyyy} | Status: {Status} | Nota: {notaTexto}";
    }
}