using SistemaCursos.Modelo;

Console.WriteLine("=== PROJETO 1: SISTEMA DE CURSOS E MATRÍCULAS ===\n");

// 1. Criação dos objetos e relacionamentos
var professor = new Professor("Carlos Mendes", "Programação Orientada a Objetos");
var material1 = new MaterialApoio("Apostila de C#", "PDF");
var material2 = new MaterialApoio("Guia de UML", "Slides");

var turma = new Turma("POO - Turma A", professor);
turma.AdicionarMaterial(material1);
turma.AdicionarMaterial(material2);

Console.WriteLine($"Turma criada: {turma.Nome} | Professor: {turma.Professor}");
Console.WriteLine("Materiais de apoio vinculados:");
foreach (var mat in turma.Materiais)
    Console.WriteLine($" - {mat}");

// 2 e 3. Comparação de duas instâncias da mesma entidade (Equals, ReferenceEquals e GetHashCode)
Console.WriteLine("\n--- Identidade Lógica vs Referência ---");
var est1 = new Estudante("RA2026", "Ana Paula");
var est1Copia = new Estudante("ra2026", "Ana Paula Silva");
var est2 = new Estudante("RA2027", "Bruno Costa");

Console.WriteLine($"est1: {est1} | est1Copia: {est1Copia}");
Console.WriteLine($"ReferenceEquals(est1, est1Copia): {ReferenceEquals(est1, est1Copia)}");
Console.WriteLine($"est1.Equals(est1Copia): {est1.Equals(est1Copia)}");
Console.WriteLine($"GetHashCode iguais? {est1.GetHashCode() == est1Copia.GetHashCode()}");

// Experimento com List<T>.Contains() e HashSet<T>
Console.WriteLine("\n--- Experimentos: List<T>.Contains() e HashSet<T> ---");
var listaEstudantes = new List<Estudante> { est1 };
Console.WriteLine($"List.Contains(est1Copia): {listaEstudantes.Contains(est1Copia)}");

var conjuntoEstudantes = new HashSet<Estudante>();
Console.WriteLine($"HashSet.Add(est1): {conjuntoEstudantes.Add(est1)}");
Console.WriteLine($"HashSet.Add(est1Copia): {conjuntoEstudantes.Add(est1Copia)} (bloqueado por Equals + GetHashCode)");
Console.WriteLine($"Total no HashSet: {conjuntoEstudantes.Count}");

// 4 e 5. Inclusão válida de ocorrências da classe associativa e listagem
Console.WriteLine("\n--- Matrículas (Classe Associativa) ---");
turma.MatricularEstudante(est1);
turma.MatricularEstudante(est2);

ExibirMatriculas(turma);

// 6. Tentativa de duplicidade
Console.WriteLine("\n--- Tentativa de Duplicidade ---");
try
{
    turma.MatricularEstudante(est1Copia);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Bloqueado corretamente: {ex.Message}");
}

// 7. Mudança de estado válida e transição proibida
Console.WriteLine("\n--- Transições de Estado ---");
turma.AtivarMatricula(est1);
turma.BuscarMatricula(est1)?.RegistrarNota(9.5m);
Console.WriteLine("Matrícula de Ana ativada e nota registrada.");

turma.CancelarMatricula(est1);
Console.WriteLine("Matrícula de Ana cancelada.");

try
{
    turma.AtivarMatricula(est1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Transição proibida bloqueada: {ex.Message}");
}

// 8. Consulta que não encontra resultado
Console.WriteLine("\n--- Consulta Inexistente ---");
var estNaoInscrito = new Estudante("RA9999", "Visitante");
var busca = turma.BuscarMatricula(estNaoInscrito);
if (busca is null)
    Console.WriteLine($"Nenhuma matrícula encontrada para {estNaoInscrito}.");

// 9. Encerramento lógico da turma
Console.WriteLine("\n--- Encerramento Lógico da Turma ---");
turma.Encerrar();
Console.WriteLine($"Turma '{turma.Nome}' encerrada? {turma.Encerrada}");
ExibirMatriculas(turma);

Console.WriteLine("\nObjetos independentes que continuam válidos após o encerramento:");
Console.WriteLine($" - Professor: {professor}");
Console.WriteLine($" - Estudante: {est2}");
Console.WriteLine($" - Material: {material1}");

static void ExibirMatriculas(Turma turma)
{
    if (turma.Matriculas.Count == 0)
    {
        Console.WriteLine("Nenhuma matrícula registrada.");
        return;
    }

    foreach (var m in turma.Matriculas)
        Console.WriteLine($" - {m}");
}
