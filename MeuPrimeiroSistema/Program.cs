// =====================================================================
// Trabalho "Meu Primeiro Sistema Interativo em C#"
// Ordem de Serviço de Assistência Técnica
// =====================================================================


Console.WriteLine("======================================================");
Console.WriteLine("   ASSISTÉNCIA TÉCNICA - NOVA ORDEM DE SERVIÇO");
Console.WriteLine("======================================================");
Console.WriteLine();

// ---------------------------------------------------------------------
// 1) ENTRADA string - nome do cliente (validação de texto vazio)
// ---------------------------------------------------------------------
Console.Write("Nome do cliente: ");
string nomeCliente = Console.ReadLine() ?? "";

while (string.IsNullOrWhiteSpace(nomeCliente))
{
    Console.WriteLine("Valor inválido. O nome nao pode ficar em branco.");
    Console.Write("Nome do cliente: ");
    nomeCliente = Console.ReadLine() ?? "";
}

// ---------------------------------------------------------------------
// 2) ENTRADA string - equipamento
// ---------------------------------------------------------------------
Console.Write("Equipamento (ex: Notebook Acer Nitro AN515): ");
string equipamento = Console.ReadLine() ?? "";

while (string.IsNullOrWhiteSpace(equipamento))
{
    Console.WriteLine("Valor inválido. Informe a descrição do equipamento.");
    Console.Write("Equipamento: ");
    equipamento = Console.ReadLine() ?? "";
}

// ---------------------------------------------------------------------
// 3) ENTRADA int - quantidade de peças (TryParse + regra > 0)
// ---------------------------------------------------------------------
int quantidadePecas;
Console.Write("Quantidade de peças a substituir (número inteiro): ");

while (!int.TryParse(Console.ReadLine(), out quantidadePecas) || quantidadePecas < 0)
{
    Console.WriteLine("Valor inválido. Digite um número inteiro maior ou igual a 0. Ex: 2");
    Console.Write("Quantidade de peças: ");
}

// ---------------------------------------------------------------------
// 4) ENTRADA decimal - valor unitário da peça (dinheiro = decimal)
// ---------------------------------------------------------------------
decimal valorPeca;
Console.Write("Valor unitario da peça em R$ (ex: 189,90): ");

while (!decimal.TryParse(Console.ReadLine(), out valorPeca) || valorPeca < 0)
{
    Console.WriteLine("Valor inválido. Digite um valor monetário, usando vírgula. Ex: 189,90");
    Console.Write("Valor unitário da peça: R$ ");
}

// ---------------------------------------------------------------------
// 5) ENTRADA double - horas de mão de obra (medida = double)
// ---------------------------------------------------------------------
double horasServico;
Console.Write("Horas estimadas de mão de obra (ex: 2,5): ");

while (!double.TryParse(Console.ReadLine(), out horasServico) || horasServico <= 0)
{
    Console.WriteLine("Valor inválido. Digite um número decimal maior que zero. Ex: 2,5");
    Console.Write("Horas estimadas de mão de obra: ");
}

// ---------------------------------------------------------------------
// 6) ENTRADA char - prioridade do atendimento (B, M ou A)
// ---------------------------------------------------------------------
char prioridade;
Console.Write("Prioridade [B]aixa, [M]edia ou [A]lta: ");

while (!char.TryParse(Console.ReadLine(), out prioridade)
       || (char.ToUpper(prioridade) != 'B'
           && char.ToUpper(prioridade) != 'M'
           && char.ToUpper(prioridade) != 'A'))
{
    Console.WriteLine("Valor inválido. Digite apenas uma letra: B, M ou A.");
    Console.Write("Prioridade [B/M/A]: ");
}

prioridade = char.ToUpper(prioridade);

// ---------------------------------------------------------------------
// 7) ENTRADA bool - equipamento ainda na garantia? (aceita s/n)
// ---------------------------------------------------------------------
bool naGarantia = false;
bool respostaValida = false;
Console.Write("O equipamento está na garantia? (s/n): ");

while (!respostaValida)
{
    string resposta = Console.ReadLine() ?? "";

    if (resposta == "s" || resposta == "S")
    {
        naGarantia = true;
        respostaValida = true;
    }
    else if (resposta == "n" || resposta == "N")
    {
        naGarantia = false;
        respostaValida = true;
    }
    else
    {
        Console.WriteLine("Valor inválido. Responda apenas com s (sim) ou n (não).");
        Console.Write("O equipamento está na garantia? (s/n): ");
    }
}

// =====================================================================
// PROCESSAMENTO - 3 calculos + uso de var + DateTime
// =====================================================================

// Calculo 1: custo total das pecas  (1o uso de var)
var custoPecas = quantidadePecas * valorPeca;

// Calculo 2: custo da mao de obra (hora tecnica fixa de R$ 80,00)
decimal valorHoraTecnica = 80.00m;
decimal custoMaoDeObra = (decimal)horasServico * valorHoraTecnica;

// Calculo 3: total com desconto (garantia isenta a mao de obra)
decimal desconto = 0m;

if (naGarantia)
{
    desconto = custoMaoDeObra;
}

decimal totalOrdem = custoPecas + custoMaoDeObra - desconto;

// Prazo de entrega estimado conforme prioridade (DateTime + if/else)
int diasPrazo = 5;

if (prioridade == 'A')
{
    diasPrazo = 1;
}
else if (prioridade == 'M')
{
    diasPrazo = 3;
}

var dataAbertura = DateTime.Now;                       // 2o uso de var
DateTime previsaoEntrega = dataAbertura.AddDays(diasPrazo);

string descricaoPrioridade = "Baixa";

if (prioridade == 'A')
{
    descricaoPrioridade = "Alta";
}
else if (prioridade == 'M')
{
    descricaoPrioridade = "Media";
}

// =====================================================================
// SAIDA - relatorio final 
// =====================================================================
Console.WriteLine();
Console.WriteLine("=============================================");
Console.WriteLine("        ORDEM DE SERVICO - RESUMO FINAL      ");
Console.WriteLine("=============================================");
Console.WriteLine($"Cliente............: {nomeCliente}");
Console.WriteLine($"Equipamento........: {equipamento}");
Console.WriteLine($"Prioridade.........: {prioridade} ({descricaoPrioridade})");
Console.WriteLine($"Na garantia........: {(naGarantia ? "Sim" : "Não")}");
Console.WriteLine("---------------------------------------------");
Console.WriteLine($"Peças..............: {quantidadePecas} x R$ {valorPeca:F2}");
Console.WriteLine($"Custo das peças....: R$ {custoPecas:F2}");
Console.WriteLine($"Mão de obra........: {horasServico:F1}h x R$ {valorHoraTecnica:F2}");
Console.WriteLine($"Custo mão de obra..: R$ {custoMaoDeObra:F2}");
Console.WriteLine($"Desconto garantia..: R$ {desconto:F2}");
Console.WriteLine("---------------------------------------------");
Console.WriteLine($"TOTAL DA ORDEM.....: R$ {totalOrdem:F2}");
Console.WriteLine($"Aberta em..........: {dataAbertura:dd/MM/yyyy HH:mm}");
Console.WriteLine($"Previsão de entrega: {previsaoEntrega:dd/MM/yyyy} ({diasPrazo} dia(s))");
Console.WriteLine("=============================================");
Console.WriteLine("Ordem registrada com sucesso. Obrigado!");
