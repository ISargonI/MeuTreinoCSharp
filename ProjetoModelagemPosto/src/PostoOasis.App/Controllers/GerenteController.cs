using PostoOasis.Core.Interfaces;
using PostoOasis.Core.Models;

namespace PostoOasis.App.Controllers;

public class GerenteController
{
    private readonly IPostoRepository _repository;

    public GerenteController(IPostoRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<Combustivel> ObterTanques() => _repository.ObterTanques();
    public IReadOnlyList<Bomba> ObterBombas() => _repository.ObterBombas();
    public IReadOnlyList<Produto> ObterProdutos() => _repository.ObterProdutos();
    public IReadOnlyList<Cliente> ObterClientes() => _repository.ObterClientes();
    public IReadOnlyList<Funcionario> ObterFuncionarios() => _repository.ObterFuncionarios();
    public IReadOnlyList<Venda> ObterVendas() => _repository.ObterVendas();
    public SessaoCaixa ObterSessaoCaixa() => _repository.ObterSessaoCaixaAtual();

    // =========================================================================
    // CRUD DE COMBUSTÍVEL - RNF10: ATUALIZAÇÃO FÁCIL DE PREÇO PELO GERENTE
    // =========================================================================
    public (bool sucesso, string mensagem) AlterarPrecoCombustivel(TipoCombustivel tipo, decimal novoPreco)
    {
        if (novoPreco <= 0) return (false, "Preço deve ser maior que zero.");

        var tanque = _repository.ObterTanquePorTipo(tipo);
        if (tanque == null) return (false, "Combustível não encontrado.");

        decimal precoAntigo = tanque.PrecoLitro;
        tanque.PrecoLitro = novoPreco;
        _repository.AtualizarTanque(tanque);

        // Atualiza preço nas bombas conectadas
        foreach (var b in _repository.ObterBombas().Where(b => b.CombustivelAssociado.Tipo == tipo))
        {
            _repository.AtualizarBomba(b);
        }

        return (true, $"[RNF10 REAJUSTE DE PREÇO] Preço do litro de {tipo} reajustado de R$ {precoAntigo:F2} para R$ {novoPreco:F2}. Sincronizado nas bombas.");
    }

    // =========================================================================
    // CRUD DE PRODUTOS DA CONVENIÊNCIA - RNF10 / RF11
    // =========================================================================
    public (bool sucesso, string mensagem) SalvarProduto(int idProduto, string nome, decimal preco, string categoria, int estoque, int estoqueMinimo)
    {
        if (string.IsNullOrWhiteSpace(nome) || preco <= 0)
            return (false, "Nome e preço válido são obrigatórios.");

        var prod = new Produto
        {
            IdProduto = idProduto,
            Nome = nome,
            Preco = preco,
            Categoria = categoria,
            QuantidadeEstoque = estoque,
            EstoqueMinimo = estoqueMinimo
        };

        _repository.SalvarProduto(prod);
        string acao = idProduto == 0 ? "cadastrado" : "atualizado";
        return (true, $"[RNF10 / RF11 SUCESSO] Produto '{nome}' {acao} com sucesso. Preço: R$ {preco:F2}.");
    }

    // =========================================================================
    // RN19: AJUSTE MANUAL DE ESTOQUE COM AUTORIZAÇÃO DO GERENTE
    // =========================================================================
    public (bool sucesso, string mensagem) RealizarAjusteEstoque(int idProduto, int novaQuantidade, string motivo, Gerente gerente)
    {
        var prod = _repository.ObterProdutoPorId(idProduto);
        if (prod == null) return (false, "Produto não encontrado.");

        _repository.RealizarAjusteEstoque(idProduto, novaQuantidade, motivo, gerente);
        return (true, $"[RN19 AUTORIZADO] Ajuste manual homologado pelo Gerente {gerente.Nome}. Produto '{prod.Nome}' agora com {novaQuantidade} un. Motivo: {motivo}.");
    }

    // =========================================================================
    // RN16 / RF19: FECHAMENTO DE CAIXA HOMOLOGADO PELO GERENTE
    // =========================================================================
    public (bool sucesso, string mensagem) HomologarFechamentoCaixa(Gerente gerente)
    {
        var sessao = _repository.ObterSessaoCaixaAtual();
        _repository.FecharSessaoCaixa(gerente);
        return (true, $"[RN16 / RF19 HOMOLOGADO] Fechamento de caixa validado pelo Gerente {gerente.Nome}. Saldo apurado no turno: R$ {sessao.SaldoTotal:F2}.");
    }

    // =========================================================================
    // RF05 / RN11: GERAR BOLETO BANCÁRIO PARA EMPRESAS
    // =========================================================================
    public (bool sucesso, string mensagem) GerarBoletoMensalEmpresa(Cliente empresa)
    {
        if (!empresa.IsEmpresa)
            return (false, "Boletos mensais são gerados exclusivamente para clientes do tipo Empresa (CNPJ).");

        return (true, $"[RF05 / RN11 BOLETO EMITIDO] Boleto bancário mensal consolidado no valor de R$ {empresa.DividaPendente:F2} gerado para {empresa.Nome} (CNPJ: {empresa.CpfCnpj}). Vencimento: Fim do mês.");
    }

    // =========================================================================
    // RF20: RELATÓRIOS GERENCIAIS DE VENDAS E ESTOQUE
    // =========================================================================
    public (decimal faturamentoConveniencia, decimal faturamentoCombustivel, decimal dividasAtivas, int produtosEstoqueBaixo) ObterTotaisRelatorio()
    {
        var vendas = _repository.ObterVendas().Where(v => v.Status == StatusVenda.Concluida);
        decimal faturamentoConv = vendas.SelectMany(v => v.ItensProdutos).Sum(i => i.Subtotal);
        decimal faturamentoComb = vendas.Where(v => v.ItemCombustivel != null).Sum(v => v.ItemCombustivel!.Subtotal);
        decimal dividas = _repository.ObterClientes().Sum(c => c.DividaPendente);
        int prodBaixo = _repository.ObterProdutos().Count(p => p.EstoqueAbaixoDoMinimo);

        return (faturamentoConv, faturamentoComb, dividas, prodBaixo);
    }
}
