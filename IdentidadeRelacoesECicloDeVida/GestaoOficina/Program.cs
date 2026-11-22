using GestaoOficina.Modelo;

Console.WriteLine("=== PROJETO 2: GESTÃO DE OFICINA MECÂNICA ===\n");

// 1. Criação dos objetos e relacionamentos
var cliente = new Cliente("Marcos Oliveira", "(45) 99999-1234");
var elevador = new Equipamento("Elevador Hidráulico", "PAT-01");
var scanner = new Equipamento("Scanner OBD2", "PAT-02");

var os = new OrdemServico("OS-2026-001", cliente);
os.AlocarEquipamento(elevador);
os.AlocarEquipamento(scanner);

Console.WriteLine($"Ordem de Serviço: {os.Numero} | Cliente: {os.Cliente}");
Console.WriteLine("Equipamentos alocados:");
foreach (var eq in os.Equipamentos)
    Console.WriteLine($" - {eq}");

// 2 e 3. Comparação de duas instâncias da mesma entidade (Equals, ReferenceEquals e GetHashCode)
Console.WriteLine("\n--- Identidade Lógica vs Referência ---");
var mec1 = new Mecanico("MEC-10", "João Ferreira");
var mec1Copia = new Mecanico("mec-10", "João F. Souza");
var mec2 = new Mecanico("MEC-20", "Ricardo Lima");

Console.WriteLine($"mec1: {mec1} | mec1Copia: {mec1Copia}");
Console.WriteLine($"ReferenceEquals(mec1, mec1Copia): {ReferenceEquals(mec1, mec1Copia)}");
Console.WriteLine($"mec1.Equals(mec1Copia): {mec1.Equals(mec1Copia)}");
Console.WriteLine($"GetHashCode iguais? {mec1.GetHashCode() == mec1Copia.GetHashCode()}");

// Experimento com List<T>.Contains()
var listaMecanicos = new List<Mecanico> { mec1 };
Console.WriteLine($"List.Contains(mec1Copia): {listaMecanicos.Contains(mec1Copia)}");

// 4 e 5. Inclusão válida de ocorrências da classe associativa e listagem
Console.WriteLine("\n--- Execuções de Serviço (Classe Associativa) ---");
os.AdicionarExecucao(mec1, "Troca de pastilhas de freio", 280.00m);
os.AdicionarExecucao(mec2, "Alinhamento e balanceamento", 150.00m);

ExibirExecucoes(os);

// 6. Tentativa de duplicidade
Console.WriteLine("\n--- Tentativa de Duplicidade ---");
try
{
    os.AdicionarExecucao(mec1Copia, "Revisão elétrica", 200.00m);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Bloqueado corretamente: {ex.Message}");
}

// 7. Mudança de estado válida e transição proibida
Console.WriteLine("\n--- Transições de Estado ---");
os.ConcluirExecucao(mec1);
Console.WriteLine("Serviço do mecânico João concluído.");

os.CancelarExecucao(mec1);
Console.WriteLine("Serviço do mecânico João cancelado.");

try
{
    os.ConcluirExecucao(mec1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Transição proibida bloqueada: {ex.Message}");
}

// 8. Consulta que não encontra resultado
Console.WriteLine("\n--- Consulta Inexistente ---");
var mecExterno = new Mecanico("MEC-99", "Visitante");
var busca = os.BuscarExecucaoPorMecanico(mecExterno);
if (busca is null)
    Console.WriteLine($"Nenhuma execução encontrada para {mecExterno}.");

// 9. Encerramento lógico da OS
Console.WriteLine("\n--- Cancelamento Lógico da Ordem de Serviço ---");
os.CancelarOrdem();
Console.WriteLine($"OS '{os.Numero}' cancelada? {os.Cancelada}");
ExibirExecucoes(os);

Console.WriteLine("\nObjetos independentes que permanecem válidos após cancelar a OS:");
Console.WriteLine($" - Cliente: {cliente}");
Console.WriteLine($" - Mecânico: {mec2}");
Console.WriteLine($" - Equipamento: {elevador}");

static void ExibirExecucoes(OrdemServico os)
{
    if (os.Execucoes.Count == 0)
    {
        Console.WriteLine("Nenhuma execução de serviço registrada.");
        return;
    }

    foreach (var e in os.Execucoes)
        Console.WriteLine($" - {e}");
}
