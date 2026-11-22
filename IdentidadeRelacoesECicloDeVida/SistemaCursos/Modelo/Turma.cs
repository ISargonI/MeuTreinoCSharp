namespace SistemaCursos.Modelo;

public class Turma
{
    private readonly List<Matricula> _matriculas = new();
    private readonly List<MaterialApoio> _materiais = new();

    public string Nome { get; }
    public Professor Professor { get; }
    public bool Encerrada { get; private set; }

    public IReadOnlyCollection<Matricula> Matriculas => _matriculas.AsReadOnly();
    public IReadOnlyCollection<MaterialApoio> Materiais => _materiais.AsReadOnly();

    public Turma(string nome, Professor professor)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da turma é obrigatório.", nameof(nome));

        Nome = nome.Trim();
        Professor = professor ?? throw new ArgumentNullException(nameof(professor));
        Encerrada = false;
    }

    public void AdicionarMaterial(MaterialApoio material)
    {
        ArgumentNullException.ThrowIfNull(material);

        if (Encerrada)
            throw new InvalidOperationException("Não é possível adicionar materiais a uma turma encerrada.");

        if (!_materiais.Contains(material))
            _materiais.Add(material);
    }

    public void MatricularEstudante(Estudante estudante)
    {
        ArgumentNullException.ThrowIfNull(estudante);

        if (Encerrada)
            throw new InvalidOperationException("Não é possível matricular em uma turma encerrada.");

        bool jaMatriculado = _matriculas.Any(m => m.Estudante.Equals(estudante));
        if (jaMatriculado)
            throw new InvalidOperationException($"Estudante {estudante.Ra} já possui matrícula nesta turma.");

        _matriculas.Add(new Matricula(estudante));
    }

    public Matricula? BuscarMatricula(Estudante estudante)
    {
        ArgumentNullException.ThrowIfNull(estudante);
        return _matriculas.FirstOrDefault(m => m.Estudante.Equals(estudante));
    }

    public void AtivarMatricula(Estudante estudante)
    {
        var matricula = BuscarMatricula(estudante)
            ?? throw new InvalidOperationException("Matrícula não encontrada.");

        matricula.Ativar();
    }

    public void CancelarMatricula(Estudante estudante)
    {
        var matricula = BuscarMatricula(estudante)
            ?? throw new InvalidOperationException("Matrícula não encontrada.");

        matricula.Cancelar();
    }

    public void Encerrar()
    {
        if (Encerrada)
            return;

        Encerrada = true;

        // Composição: matrículas pendentes são canceladas ao encerrar a turma.
        foreach (var matricula in _matriculas)
        {
            if (matricula.Status == StatusMatricula.Pendente)
                matricula.Cancelar();
        }
    }
}