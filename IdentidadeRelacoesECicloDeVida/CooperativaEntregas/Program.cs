using CooperativaEntregas.Modelo;

Console.WriteLine("=== PROJETO 3: COOPERATIVA DE ENTREGAS ===\n");

// 1. Criação dos objetos e relacionamentos
var coordenador = new Coordenador("Fernanda Alves", "Manhã");
var moto = new Veiculo("ABC-1D23", "Honda CG 160");
var furgao = new Veiculo("XYZ-9E87", "Fiat Fiorino");

var rota = new RotaEntrega("ROTA-SUL-01", coordenador);
rota.VincularVeiculo(moto);
rota.VincularVeiculo(furgao);

Console.WriteLine($"Rota: {rota.CodigoRota} | Coordenador: {rota.Coordenador}");
Console.WriteLine("Veículos vinculados:");
foreach (var v in rota.Veiculos)
    Console.WriteLine($" - {v}");

// 2 e 3. Comparação de duas instâncias da mesma entidade (Id estável)
Console.WriteLine("\n--- Identidade Lógica (Guid) vs Referência ---");
var idCompartilhado = Guid.NewGuid();
var ent1 = new Entregador("Lucas Pereira", "111.222.333-44", idCompartilhado);
var ent1MesmaEntidade = new Entregador("Lucas P. Silva", "111.222.333-44", idCompartilhado);
var ent2 = new Entregador("Mariana Rocha", "555.666.777-88");

Console.WriteLine($"ent1: {ent1}");
Console.WriteLine($"ent1MesmaEntidade: {ent1MesmaEntidade}");
Console.WriteLine($"ReferenceEquals(ent1, ent1MesmaEntidade): {ReferenceEquals(ent1, ent1MesmaEntidade)}");
Console.WriteLine($"ent1.Equals(ent1MesmaEntidade): {ent1.Equals(ent1MesmaEntidade)}");
Console.WriteLine($"GetHashCode iguais? {ent1.GetHashCode() == ent1MesmaEntidade.GetHashCode()}");

// Experimento com List<T>.Contains()
var listaEntregadores = new List<Entregador> { ent1 };
Console.WriteLine($"List.Contains(ent1MesmaEntidade): {listaEntregadores.Contains(ent1MesmaEntidade)}");

// 4 e 5. Inclusão válida de ocorrências da classe associativa e listagem
Console.WriteLine("\n--- Registros de Entrega (Classe Associativa) ---");
rota.RegistrarEntrega(ent1, "Av. Brasil, 1200", 18.50m);
rota.RegistrarEntrega(ent2, "Rua Paraguai, 450", 22.00m);

ExibirRegistros(rota);

// 6. Tentativa de duplicidade (novo Guid, mas mesmo Documento — regra separada da identidade)
Console.WriteLine("\n--- Tentativa de Duplicidade (Regra de Documento) ---");
var entDuplicadoPorDoc = new Entregador("Lucas Outro Cadastro", "111.222.333-44");
Console.WriteLine($"ent1.Equals(entDuplicadoPorDoc): {ent1.Equals(entDuplicadoPorDoc)} (Ids diferentes)");
try
{
    rota.RegistrarEntrega(entDuplicadoPorDoc, "Rua Argentina, 90", 15.00m);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Bloqueado pela regra de negócio: {ex.Message}");
}

// 7. Mudança de estado válida e transição proibida
Console.WriteLine("\n--- Transições de Estado ---");
rota.ConfirmarEntrega(ent1);
Console.WriteLine("Entrega de Lucas marcada como Entregue.");

rota.CancelarEntrega(ent1);
Console.WriteLine("Entrega de Lucas cancelada.");

try
{
    rota.ConfirmarEntrega(ent1);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Transição proibida bloqueada: {ex.Message}");
}

// 8. Consulta que não encontra resultado
Console.WriteLine("\n--- Consulta Inexistente ---");
var entForaDaRota = new Entregador("Paulo Henrique", "999.888.777-66");
var busca = rota.BuscarRegistroPorEntregador(entForaDaRota);
if (busca is null)
    Console.WriteLine($"Nenhum registro encontrado para {entForaDaRota.Nome}.");

// 9. Encerramento lógico da rota
Console.WriteLine("\n--- Encerramento Lógico da Rota ---");
rota.EncerrarRota();
Console.WriteLine($"Rota '{rota.CodigoRota}' encerrada? {rota.Encerrada}");
ExibirRegistros(rota);

Console.WriteLine("\nObjetos independentes que continuam válidos após encerrar a rota:");
Console.WriteLine($" - Coordenador: {coordenador}");
Console.WriteLine($" - Entregador: {ent2}");
Console.WriteLine($" - Veículo: {moto}");

static void ExibirRegistros(RotaEntrega rota)
{
    if (rota.Registros.Count == 0)
    {
        Console.WriteLine("Nenhum registro de entrega cadastrado.");
        return;
    }

    foreach (var r in rota.Registros)
        Console.WriteLine($" - {r}");
}
