using PostoOasis.Core.Interfaces;
using PostoOasis.Core.Models;

namespace PostoOasis.App.Controllers;

public class CaixaController
{
    private readonly IPostoRepository _repository;

    public CaixaController(IPostoRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<Produto> ObterProdutosConveniencia() => _repository.ObterProdutos();
    public IReadOnlyList<Cliente> ObterClientes() => _repository.ObterClientes();
    public IReadOnlyList<Bomba> ObterBombasComAbastecimento() => _repository.ObterBombas().Where(b => b.LitrosAbastecidos > 0).ToList();
    public IReadOnlyList<Venda> ObterHistoricoVendas() => _repository.ObterVendas();
    public SessaoCaixa ObterSessaoCaixa() => _repository.ObterSessaoCaixaAtual();

    public (bool sucesso, string mensagem, Venda? venda) FinalizarVenda(
        Funcionario operador,
        Cliente? cliente,
        FormaPagamento forma,
        List<ItemVenda> itens,
        Bomba? bombaPendente,
        string? numeroCheque = null,
        string? bancoCheque = null)
    {
        decimal total = itens.Sum(i => i.Subtotal) + (bombaPendente?.ValorTotalCalculado ?? 0);
        if (total <= 0)
            return (false, "Nenhum item ou abastecimento selecionado.", null);

        DateTime agora = DateTime.Now;

        // RN10 e RN20: Venda a Prazo
        if (forma == FormaPagamento.APrazo)
        {
            if (cliente == null)
                return (false, "[RF04 / RN10 BLOQUEIO] Venda a prazo permitida exclusivamente para clientes com cadastro completo na base de dados!", null);

            if (cliente.EstaInadimplente(agora) || (cliente.DividaPendente > 0 && cliente.DataVencimentoDivida.HasValue && agora.Date > cliente.DataVencimentoDivida.Value.Date))
                return (false, $"[RF04 / RN20 BLOQUEIO TOTAL] O cliente/empresa '{cliente.Nome}' está INADIMPLENTE (Dívida vencida de R$ {cliente.DividaPendente:F2} em {cliente.DataVencimentoDivida:dd/MM/yyyy}). Compras a prazo estão ESTRITAMENTE BLOQUEADAS!", null);
        }

        // RN18: Cheque
        if (forma == FormaPagamento.Cheque)
        {
            if (cliente == null || !cliente.PodePagarComCheque(agora))
                return (false, "[RN18 BLOQUEIO] Pagamento em cheque permitido apenas para clientes cadastrados há mais de 3 meses e sem histórico de atrasos!", null);
        }

        var novaVenda = new Venda
        {
            Operador = operador,
            Cliente = cliente,
            ItensProdutos = new List<ItemVenda>(itens),
            Pagamento = new Pagamento
            {
                Valor = total,
                Forma = forma,
                NumeroCheque = numeroCheque,
                BancoCheque = bancoCheque
            }
        };

        if (bombaPendente != null)
        {
            var frentistaPista = _repository.ObterFuncionarios().FirstOrDefault(f => f.Cargo == "Frentista") ?? operador;
            novaVenda.Frentista = frentistaPista;
            novaVenda.ItemCombustivel = new ItemCombustivel
            {
                Bomba = bombaPendente,
                Litros = bombaPendente.LitrosAbastecidos,
                PrecoLitro = bombaPendente.CombustivelAssociado.PrecoLitro
            };
            bombaPendente.LitrosAbastecidos = 0;
            bombaPendente.Status = StatusBomba.Disponivel;
            bombaPendente.BicoNoColdre = true;
            _repository.AtualizarBomba(bombaPendente);
        }

        var vendaSalva = _repository.RegistrarVenda(novaVenda);

        string feedback = $"[RN01] Venda #{vendaSalva.IdVenda} concluída com sucesso! Total: R$ {vendaSalva.ValorTotal:F2} via {forma}.";

        // RN03: Vale-lavagem a cada R$ 200
        if (vendaSalva.BonusLavagemGerado != null)
        {
            feedback += $"\n⭐ [RN03 VALE-LAVAGEM GERADO] Voucher: {vendaSalva.BonusLavagemGerado.CodigoVoucher} (Válido por 30 dias até {vendaSalva.BonusLavagemGerado.DataValidade:dd/MM/yyyy}).";
        }

        return (true, feedback, vendaSalva);
    }

    public (bool sucesso, string mensagem) EmitirNotaFiscal(int idVenda)
    {
        try
        {
            var nf = _repository.EmitirNotaFiscal(idVenda);
            return (true, $"[RF03 NF-e AUTORIZADA] Chave: {nf.ChaveAcesso} | Número: {nf.NumeroNF} | Valor: R$ {nf.ValorTotal:F2} para {nf.NomeDestinatario}.");
        }
        catch (Exception ex)
        {
            return (false, $"Erro ao emitir NF-e: {ex.Message}");
        }
    }

    public (bool sucesso, string mensagem) CancelarVenda(int idVenda, string motivo)
    {
        // RN17: Cancelamento de venda ou estorno só no prazo máximo de 30 minutos
        bool ok = _repository.CancelarVenda(idVenda, motivo, DateTime.Now);
        if (!ok)
            return (false, "[RN17 BLOQUEIO] Cancelamento não permitido: A venda ultrapassou o limite máximo de 30 minutos desde a emissão ou já foi cancelada!");

        return (true, $"[RN17 SUCESSO] Venda #{idVenda} cancelada e itens devolvidos ao estoque dentro da tolerância de 30 minutos.");
    }

    public void RegistrarPonto(Funcionario f, TipoMarcacaoPonto tipo) => _repository.RegistrarPonto(f, tipo);

    // RF06: Gerenciar Clientes (Cadastro pelo Caixa ou Frentista)
    public (bool sucesso, string mensagem, Cliente? cliente) CadastrarCliente(
        string nome,
        string cpfCnpj,
        string endereco,
        string telefone,
        string email)
    {
        if (string.IsNullOrWhiteSpace(nome) || string.IsNullOrWhiteSpace(cpfCnpj))
            return (false, "Nome e CPF/CNPJ são obrigatórios para cadastro!", null);

        string limpo = new string(cpfCnpj.Where(char.IsDigit).ToArray());
        if (limpo.Length != 11 && limpo.Length != 14)
            return (false, "Documento deve conter 11 dígitos (CPF) ou 14 dígitos (CNPJ).", null);

        var existente = _repository.ObterClientePorDoc(limpo);
        if (existente != null)
            return (false, $"Já existe um cliente cadastrado com este documento ({existente.Nome}).", null);

        var novo = new Cliente
        {
            Nome = nome.Trim(),
            CpfCnpj = limpo,
            Endereco = string.IsNullOrWhiteSpace(endereco) ? "Endereço não informado" : endereco.Trim(),
            Telefone = string.IsNullOrWhiteSpace(telefone) ? "Telefone não informado" : telefone.Trim(),
            Email = string.IsNullOrWhiteSpace(email) ? "email@cliente.com" : email.Trim(),
            DataCadastro = DateTime.Now
        };

        _repository.SalvarCliente(novo);
        return (true, $"[RF06] Cliente '{novo.Nome}' cadastrado com sucesso e apto para transações!", novo);
    }

    public void QuitarDivida(int idCliente)
    {
        _repository.QuitarDividaCliente(idCliente);
    }
}
