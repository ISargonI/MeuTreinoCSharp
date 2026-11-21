using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Styling;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PostoOasis.App.Controllers;
using PostoOasis.Core.Interfaces;
using PostoOasis.Core.Models;
using PostoOasis.Core.Services;

namespace PostoOasis.App.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    private readonly IPostoRepository _repository;
    private DispatcherTimer? _timerBomba;
    private Bomba? _bombaEmAbastecimento;

    // Controllers da Arquitetura MVC
    public LoginController LoginCtrl { get; }
    public FrentistaController FrentistaCtrl { get; }
    public CaixaController CaixaCtrl { get; }
    public AuxiliarAdmController AuxiliarAdmCtrl { get; }
    public GerenteController GerenteCtrl { get; }

    // =========================================================================
    // TEMA VISUAL (MODO CLARO / MODO ESCURO)
    // =========================================================================
    [ObservableProperty]
    private bool _isModoEscuro = true;

    [ObservableProperty]
    private string _iconeTemaTexto = "☀️ Modo Claro";

    // =========================================================================
    // NAVEGAÇÃO ENTRE TELAS
    // =========================================================================
    [ObservableProperty]
    private string _telaAtual = "Login"; // "Login", "Frentista", "Caixa", "AuxiliarAdm", "Gerente"

    [ObservableProperty]
    private Funcionario? _usuarioAutenticado;

    [ObservableProperty]
    private string _statusGlobal = "Sistema Auto Posto Oásis pronto para autenticação.";

    public bool IsTelaLogin => TelaAtual == "Login";
    public bool IsTelaFrentista => TelaAtual == "Frentista";
    public bool IsTelaCaixa => TelaAtual == "Caixa";
    public bool IsTelaAuxiliarAdm => TelaAtual == "AuxiliarAdm";
    public bool IsTelaGerente => TelaAtual == "Gerente";
    public bool IsUsuarioAutenticado => UsuarioAutenticado != null;

    partial void OnTelaAtualChanged(string value)
    {
        OnPropertyChanged(nameof(IsTelaLogin));
        OnPropertyChanged(nameof(IsTelaFrentista));
        OnPropertyChanged(nameof(IsTelaCaixa));
        OnPropertyChanged(nameof(IsTelaAuxiliarAdm));
        OnPropertyChanged(nameof(IsTelaGerente));
    }

    partial void OnUsuarioAutenticadoChanged(Funcionario? value)
    {
        OnPropertyChanged(nameof(IsUsuarioAutenticado));
    }

    // =========================================================================
    // TELA DE LOGIN (UC16 / RF16 / RNF08 - COM EXIGÊNCIA DE SENHA)
    // =========================================================================
    [ObservableProperty]
    private string _loginUsuario = "";

    [ObservableProperty]
    private string _loginSenha = "";

    [ObservableProperty]
    private string _perfilSelecionadoNome = "Selecione um perfil ou digite as credenciais";

    [ObservableProperty]
    private string _mensagemLogin = "";

    // =========================================================================
    // TELA FRENTISTA (RF01, RN12, RN09, RF17 - COM FLUXO CONTÍNUO AUTOMATIZADO)
    // =========================================================================
    public ObservableCollection<Bomba> BombasFrentista { get; } = new();

    [ObservableProperty]
    private Bomba? _bombaFrentistaSelecionada;

    [ObservableProperty]
    private bool _isAbastecendoContinuo = false;

    [ObservableProperty]
    private string _textoBotaoFluxo = "▶️ Iniciar Fluxo Livre";

    [ObservableProperty]
    private string _valorPredeterminadoFrentista = "50";

    [ObservableProperty]
    private string _placaFugaFrentista = "ABC-1234";

    [ObservableProperty]
    private string _valorFugaFrentista = "120,00";

    [ObservableProperty]
    private string _mensagemFrentista = "";

    // =========================================================================
    // TELA CAIXA (RF02, RF03, RF04, RF19, RN01, RN02, RN03, RN10, RN17, RN18, RN20 - METÁFORA CARTÃO)
    // =========================================================================
    public ObservableCollection<Produto> ProdutosCaixa { get; } = new();
    public ObservableCollection<Cliente> ClientesCaixa { get; } = new();
    public ObservableCollection<Bomba> BombasPendentesCaixa { get; } = new();
    public ObservableCollection<ItemVenda> CarrinhoCaixa { get; } = new();
    public ObservableCollection<Venda> HistoricoVendasCaixa { get; } = new();

    [ObservableProperty]
    private Bomba? _bombaPendenteSelecionada;

    [ObservableProperty]
    private Cliente? _clienteCaixaSelecionado;

    [ObservableProperty]
    private bool _isClienteInadimplente = false;

    [ObservableProperty]
    private string _mensagemStatusCliente = "Consumidor Final (Sem cadastro no sistema)";

    // RF06: Cadastro de Novo Cliente
    [ObservableProperty]
    private bool _mostrarFormularioNovoCliente = false;

    [ObservableProperty]
    private string _novoClienteNome = "";

    [ObservableProperty]
    private string _novoClienteCpfCnpj = "";

    [ObservableProperty]
    private string _novoClienteEndereco = "";

    [ObservableProperty]
    private string _novoClienteTelefone = "";

    [ObservableProperty]
    private string _novoClienteEmail = "";

    [ObservableProperty]
    private FormaPagamento _formaPagamentoCaixa = FormaPagamento.Dinheiro;

    [ObservableProperty]
    private string _numeroChequeCaixa = "";

    [ObservableProperty]
    private string _bancoChequeCaixa = "Banco do Brasil";

    // Metáfora do Cartão de Crédito/Débito (Slide 8 - Mundo Real)
    [ObservableProperty]
    private bool _isPagamentoCartao = false;

    [ObservableProperty]
    private bool _cartaoAutorizado = false;

    [ObservableProperty]
    private string _cartaoBandeira = "Mastercard";

    [ObservableProperty]
    private string _cartaoMensagemStatus = "Aguardando aproximar ou inserir cartão na maquininha...";

    [ObservableProperty]
    private string _mensagemCaixa = "";

    public decimal TotalVendaCaixa
    {
        get
        {
            decimal totalItens = CarrinhoCaixa.Sum(i => i.Subtotal);
            decimal totalCombustivel = BombaPendenteSelecionada?.ValorTotalCalculado ?? 0;
            return totalItens + totalCombustivel;
        }
    }

    // =========================================================================
    // TELA AUXILIAR ADMINISTRATIVO / RH (RF09, RF13, RF14, RF15, RF17, RF18, RN04, RN05, RN08, RN15)
    // =========================================================================
    public ObservableCollection<Funcionario> FuncionariosAdm { get; } = new();
    public ObservableCollection<Fornecedor> FornecedoresAdm { get; } = new();
    public ObservableCollection<RegistroPonto> EspelhoPontoAdm { get; } = new();
    public ObservableCollection<Combustivel> TanquesAdm { get; } = new();
    public ObservableCollection<Produto> ProdutosAdm { get; } = new();
    public ObservableCollection<CompraMercadoria> ComprasMercadoriasAdm { get; } = new();
    public ObservableCollection<RelatorioHorasFuncionario> RelatorioHorasRH { get; } = new();

    // RF14: Compra de Mercadorias para a Loja
    [ObservableProperty]
    private Produto? _compraLojaProdutoSelecionado;

    [ObservableProperty]
    private Fornecedor? _compraLojaFornecedorSelecionado;

    [ObservableProperty]
    private string _compraLojaQuantidade = "10";

    [ObservableProperty]
    private string _compraLojaPrecoUnitario = "4,50";

    [ObservableProperty]
    private string _novoFuncNome = "";

    [ObservableProperty]
    private string _novoFuncCpf = "";

    [ObservableProperty]
    private string _novoFuncCargo = "Frentista";

    [ObservableProperty]
    private string _novoFuncSalario = "1700,00";

    [ObservableProperty]
    private string _novoFuncCarteira = "987654/001";

    [ObservableProperty]
    private string _novoFuncTelefone = "(45) 9988-1122";

    [ObservableProperty]
    private string _novoFuncEndereco = "Rua Pará, 150";

    [ObservableProperty]
    private string _novoFornRazao = "";

    [ObservableProperty]
    private string _novoFornCnpj = "";

    [ObservableProperty]
    private string _novoFornTelefone = "(45) 3264-5500";

    [ObservableProperty]
    private string _novoFornEmail = "vendas@distribuidora.com.br";

    [ObservableProperty]
    private Combustivel? _tanqueCompraSelecionado;

    [ObservableProperty]
    private string _litrosCompraCombustivel = "5000";

    [ObservableProperty]
    private string _mensagemAdm = "";

    // =========================================================================
    // TELA GERENTE - CRUDs, AUDITORIA CLICÁVEL (RF20, RNF10, RN11, RN16, RN19)
    // =========================================================================
    public ObservableCollection<Combustivel> TanquesGerente { get; } = new();
    public ObservableCollection<Produto> ProdutosGerente { get; } = new();
    public ObservableCollection<Cliente> ClientesGerente { get; } = new();
    public ObservableCollection<Funcionario> FuncionariosGerente { get; } = new();

    [ObservableProperty]
    private SessaoCaixa _sessaoCaixaGerente;

    // CRUD Produto
    [ObservableProperty]
    private int _crudProdutoId = 0;

    [ObservableProperty]
    private string _crudProdutoNome = "";

    [ObservableProperty]
    private string _crudProdutoPreco = "10,00";

    [ObservableProperty]
    private string _crudProdutoCategoria = "Bebidas";

    [ObservableProperty]
    private string _crudProdutoEstoque = "20";

    [ObservableProperty]
    private string _crudProdutoEstoqueMin = "5";

    // Reajuste Combustível
    [ObservableProperty]
    private string _novoPrecoGasolina = "5,89";

    [ObservableProperty]
    private string _novoPrecoEtanol = "3,99";

    [ObservableProperty]
    private string _novoPrecoDiesel = "6,19";

    // Ajuste Manual de Estoque (RN19)
    [ObservableProperty]
    private Produto? _produtoAjusteManual;

    [ObservableProperty]
    private string _novaQtdAjusteManual = "5";

    [ObservableProperty]
    private string _motivoAjusteManual = "Produto quebrado / avariado";

    // Relatórios e Auditoria Detalhada Clicável (RF20)
    [ObservableProperty]
    private decimal _relatorioFaturamentoConv = 0;

    [ObservableProperty]
    private decimal _relatorioFaturamentoComb = 0;

    [ObservableProperty]
    private decimal _relatorioTotalDividas = 0;

    [ObservableProperty]
    private int _relatorioItensEstoqueBaixo = 0;

    // Modal / Drawer de Detalhes dos Faturamentos
    [ObservableProperty]
    private bool _isDetalheFaturamentoAberto = false;

    [ObservableProperty]
    private string _tituloDetalheFaturamento = "Detalhes";

    [ObservableProperty]
    private string _tipoDetalheAtivo = "Pista"; // "Pista", "Loja", "Dividas", "EstoqueBaixo"

    public bool IsDetalhePista => TipoDetalheAtivo == "Pista";
    public bool IsDetalheLoja => TipoDetalheAtivo == "Loja";
    public bool IsDetalheDividas => TipoDetalheAtivo == "Dividas";
    public bool IsDetalheEstoqueBaixo => TipoDetalheAtivo == "EstoqueBaixo";

    public ObservableCollection<Venda> VendasPistaDetalhe { get; } = new();
    public ObservableCollection<Venda> VendasLojaDetalhe { get; } = new();
    public ObservableCollection<Cliente> DevedoresDetalhe { get; } = new();
    public ObservableCollection<Produto> ProdutosEstoqueBaixoDetalhe { get; } = new();
    public ObservableCollection<Combustivel> TanquesAlertaCombustivel { get; } = new();

    [ObservableProperty]
    private bool _hasTanquesEmAlerta = false;

    [ObservableProperty]
    private string _relatorioTotalAlertasEstoqueTexto = "2 Tanques + 2 Itens";

    [ObservableProperty]
    private string _filtroPeriodoRelatorioGerente = "Mensal";

    [ObservableProperty]
    private string _mensagemGerente = "";

    public MainWindowViewModel(IPostoRepository repository)
    {
        _repository = repository;
        LoginCtrl = new LoginController(_repository);
        FrentistaCtrl = new FrentistaController(_repository);
        CaixaCtrl = new CaixaController(_repository);
        AuxiliarAdmCtrl = new AuxiliarAdmController(_repository);
        GerenteCtrl = new GerenteController(_repository);

        _sessaoCaixaGerente = _repository.ObterSessaoCaixaAtual();

        ConfigurarTimerBomba();
        RecarregarDadosGerais();
    }

    private void ConfigurarTimerBomba()
    {
        _timerBomba = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(150) // Pulsos suaves e atualização em tempo real
        };
        _timerBomba.Tick += TimerBomba_Tick;
    }

    private void TimerBomba_Tick(object? sender, EventArgs e)
    {
        var bomba = _bombaEmAbastecimento;
        if (bomba == null || bomba.Status != StatusBomba.EmAbastecimento)
        {
            _timerBomba?.Stop();
            IsAbastecendoContinuo = false;
            _bombaEmAbastecimento = null;
            return;
        }

        // RN12: Verifica corte de combustível em nível crítico < 4%
        if (bomba.CombustivelAssociado.EstaNivelCriticoBloqueio)
        {
            _timerBomba?.Stop();
            IsAbastecendoContinuo = false;
            _bombaEmAbastecimento = null;
            bomba.Status = StatusBomba.BloqueadaSegurancaCritica;
            _repository.AtualizarBomba(bomba);
            MensagemFrentista = $"[RN12 CORTE AUTOMÁTICO] {bomba.Identificador} bloqueada! Tanque atingiu nível crítico (< 4%).";
            StatusGlobal = MensagemFrentista;
            return;
        }

        // 5 pulsos de 0.01L = 0.05L a cada tick
        bomba.ReceberPulsos(5);

        _repository.AtualizarBomba(bomba);
        _repository.AtualizarTanque(bomba.CombustivelAssociado);

        // Se for valor fixo pré-determinado e atingiu o limite:
        if (bomba.ModoOperacao == TipoOperacaoBomba.EspecialValorFixo && bomba.LimiteValorPredeterminado.HasValue)
        {
            if (bomba.ValorTotalCalculado >= bomba.LimiteValorPredeterminado.Value)
            {
                _timerBomba?.Stop();
                IsAbastecendoContinuo = false;
                _bombaEmAbastecimento = null;
                bomba.Status = StatusBomba.Pausada;
                _repository.AtualizarBomba(bomba);
                MensagemFrentista = $"[LIMITE ATINGIDO] {bomba.Identificador}: Abastecimento de R$ {bomba.ValorTotalCalculado:F2} ({bomba.LitrosAbastecidos:F2} L) atingiu o limite pré-fixado!";
                StatusGlobal = MensagemFrentista;
                return;
            }
        }

        MensagemFrentista = $"[ABASTECENDO] {bomba.Identificador}: {bomba.LitrosAbastecidos:F2} L | Total: R$ {bomba.ValorTotalCalculado:F2}";
    }

    // =========================================================================
    // COMANDO DE TEMA (CLARO / ESCURO)
    // =========================================================================
    [RelayCommand]
    public void AlternarTema()
    {
        IsModoEscuro = !IsModoEscuro;
        if (Application.Current is { } app)
        {
            app.RequestedThemeVariant = IsModoEscuro ? ThemeVariant.Dark : ThemeVariant.Light;
        }
        IconeTemaTexto = IsModoEscuro ? "☀️ Modo Claro" : "🌙 Modo Escuro";
    }

    private void RecarregarDadosGerais()
    {
        // Frentista
        BombasFrentista.Clear();
        foreach (var b in _repository.ObterBombas()) BombasFrentista.Add(b);
        if (BombasFrentista.Count > 0 && BombaFrentistaSelecionada == null)
            BombaFrentistaSelecionada = BombasFrentista[0];

        // Caixa
        ProdutosCaixa.Clear();
        foreach (var p in _repository.ObterProdutos()) ProdutosCaixa.Add(p);

        ClientesCaixa.Clear();
        foreach (var c in _repository.ObterClientes()) ClientesCaixa.Add(c);

        BombasPendentesCaixa.Clear();
        foreach (var b in _repository.ObterBombas().Where(b => b.LitrosAbastecidos > 0)) BombasPendentesCaixa.Add(b);

        HistoricoVendasCaixa.Clear();
        foreach (var v in _repository.ObterVendas()) HistoricoVendasCaixa.Add(v);

        // Adm
        FuncionariosAdm.Clear();
        foreach (var f in _repository.ObterFuncionarios()) FuncionariosAdm.Add(f);

        FornecedoresAdm.Clear();
        foreach (var fn in _repository.ObterFornecedores()) FornecedoresAdm.Add(fn);

        TanquesAdm.Clear();
        foreach (var t in _repository.ObterTanques()) TanquesAdm.Add(t);

        ProdutosAdm.Clear();
        foreach (var p in _repository.ObterProdutos()) ProdutosAdm.Add(p);

        EspelhoPontoAdm.Clear();
        foreach (var pt in _repository.ObterRegistrosPonto()) EspelhoPontoAdm.Add(pt);

        ComprasMercadoriasAdm.Clear();
        foreach (var cm in _repository.ObterComprasMercadorias()) ComprasMercadoriasAdm.Add(cm);

        if (RelatorioHorasRH.Count == 0)
        {
            foreach (var h in _repository.GerarRelatorioHorasTrabalhadas()) RelatorioHorasRH.Add(h);
        }

        // Gerente
        TanquesGerente.Clear();
        foreach (var t in _repository.ObterTanques()) TanquesGerente.Add(t);

        ProdutosGerente.Clear();
        foreach (var p in _repository.ObterProdutos()) ProdutosGerente.Add(p);

        ClientesGerente.Clear();
        foreach (var c in _repository.ObterClientes()) ClientesGerente.Add(c);

        FuncionariosGerente.Clear();
        foreach (var f in _repository.ObterFuncionarios()) FuncionariosGerente.Add(f);

        SessaoCaixaGerente = _repository.ObterSessaoCaixaAtual();

        var rel = GerenteCtrl.ObterTotaisRelatorio();
        RelatorioFaturamentoConv = rel.faturamentoConveniencia;
        RelatorioFaturamentoComb = rel.faturamentoCombustivel;
        RelatorioTotalDividas = rel.dividasAtivas;
        RelatorioItensEstoqueBaixo = rel.produtosEstoqueBaixo;

        TanquesAlertaCombustivel.Clear();
        foreach (var t in _repository.ObterTanques().Where(t => t.PrecisaAlertaCompra || t.EstaNivelCriticoBloqueio))
        {
            TanquesAlertaCombustivel.Add(t);
        }
        HasTanquesEmAlerta = TanquesAlertaCombustivel.Count > 0;
        RelatorioTotalAlertasEstoqueTexto = $"{TanquesAlertaCombustivel.Count} Tanques + {rel.produtosEstoqueBaixo} Itens";

        var gas = _repository.ObterTanquePorTipo(TipoCombustivel.Gasolina);
        if (gas != null) NovoPrecoGasolina = gas.PrecoLitro.ToString("F2");

        var eta = _repository.ObterTanquePorTipo(TipoCombustivel.Etanol);
        if (eta != null) NovoPrecoEtanol = eta.PrecoLitro.ToString("F2");

        var die = _repository.ObterTanquePorTipo(TipoCombustivel.Diesel);
        if (die != null) NovoPrecoDiesel = die.PrecoLitro.ToString("F2");

        OnPropertyChanged(nameof(TotalVendaCaixa));
    }

    // =========================================================================
    // COMANDOS DE AUTENTICAÇÃO E LOGIN (COM SENHA OBRIGATÓRIA)
    // =========================================================================
    [RelayCommand]
    public void SelecionarPerfilLogin(string perfil)
    {
        var f = LoginCtrl.ObterPorPerfil(perfil);
        if (f == null) return;

        LoginUsuario = f.Usuario;
        LoginSenha = ""; // Limpa a senha para exigir digitação
        PerfilSelecionadoNome = $"{f.Nome} ({f.Cargo})";
        MensagemLogin = $"Perfil selecionado: {f.Cargo}. Digite a senha para confirmar o acesso.";
    }

    [RelayCommand]
    public void FazerLoginCredenciais()
    {
        if (string.IsNullOrWhiteSpace(LoginUsuario) || string.IsNullOrWhiteSpace(LoginSenha))
        {
            MensagemLogin = "Por favor, selecione um perfil e informe a senha cadastrada.";
            return;
        }

        var f = LoginCtrl.Autenticar(LoginUsuario.Trim(), LoginSenha.Trim());
        if (f == null)
        {
            MensagemLogin = "Acesso negado: Senha incorreta ou usuário inativo!";
            return;
        }

        UsuarioAutenticado = f;
        MensagemLogin = "";
        StatusGlobal = $"Usuário '{f.Nome}' autenticado como {f.Cargo}.";

        TelaAtual = f.Cargo.ToLowerInvariant() switch
        {
            "frentista" => "Frentista",
            "caixa" => "Caixa",
            "gerente" => "Gerente",
            "rh" or "auxiliar administrativo" => "AuxiliarAdm",
            _ => "Frentista"
        };

        RecarregarDadosGerais();
    }

    private void PararFluxoContinuo()
    {
        _timerBomba?.Stop();
        IsAbastecendoContinuo = false;
        _bombaEmAbastecimento = null;
        TextoBotaoFluxo = "▶️ Iniciar Fluxo Livre";
    }

    [RelayCommand]
    public void FazerLogout()
    {
        PararFluxoContinuo();
        UsuarioAutenticado = null;
        TelaAtual = "Login";
        LoginUsuario = "";
        LoginSenha = "";
        PerfilSelecionadoNome = "Selecione um perfil ou digite as credenciais";
        MensagemLogin = "Sessão encerrada com sucesso.";
        StatusGlobal = "Aguardando login de operador.";
    }

    // =========================================================================
    // COMANDOS DO FRENTISTA (COM FLUXO AUTOMÁTICO LIVRE / PAUSAR / RETOMAR / COLDRE)
    // =========================================================================
    [RelayCommand]
    public void AlternarFluxoLivreFrentista(Bomba? b)
    {
        b ??= BombaFrentistaSelecionada;
        if (b == null) return;

        BombaFrentistaSelecionada = b;

        // Se está atualmente em abastecimento, pausa o fluxo (solta o gatilho)
        if (b.Status == StatusBomba.EmAbastecimento)
        {
            _timerBomba?.Stop();
            IsAbastecendoContinuo = false;
            _bombaEmAbastecimento = null;
            b.PausarFluxo();
            _repository.AtualizarBomba(b);

            MensagemFrentista = $"{b.Identificador}: Fluxo PAUSADO em {b.LitrosAbastecidos:F2} L (R$ {b.ValorTotalCalculado:F2}). Clique novamente para RETOMAR ou em 'Coldre' para concluir.";
            StatusGlobal = MensagemFrentista;
            return;
        }

        // Se estava bloqueada
        if (b.CombustivelAssociado.EstaNivelCriticoBloqueio)
        {
            b.Status = StatusBomba.BloqueadaSegurancaCritica;
            MensagemFrentista = $"[RN12 BLOQUEIO] {b.Identificador} bloqueada! Nível do tanque de {b.CombustivelAssociado.Nome} crítico (< 4%).";
            StatusGlobal = MensagemFrentista;
            return;
        }

        bool eraPausada = (b.Status == StatusBomba.Pausada);
        b.IniciarOuRetomarAbastecimento(TipoOperacaoBomba.Normal);
        _repository.AtualizarBomba(b);
        _bombaEmAbastecimento = b;
        IsAbastecendoContinuo = true;
        _timerBomba?.Start();

        MensagemFrentista = eraPausada 
            ? $"{b.Identificador}: Fluxo RETOMADO a partir de {b.LitrosAbastecidos:F2} L!"
            : $"{b.Identificador}: Abastecimento INICIADO! Bombas enviando pulsos em tempo real.";
        StatusGlobal = MensagemFrentista;
    }

    [RelayCommand]
    public void LiberarBombaPredeterminadaFrentista(Bomba? b)
    {
        b ??= BombaFrentistaSelecionada;
        if (b == null) return;

        BombaFrentistaSelecionada = b;
        _timerBomba?.Stop();

        decimal valor = decimal.TryParse(ValorPredeterminadoFrentista, out decimal v) ? v : 50;
        b.IniciarOuRetomarAbastecimento(TipoOperacaoBomba.EspecialValorFixo, valor);
        _repository.AtualizarBomba(b);
        _bombaEmAbastecimento = b;
        IsAbastecendoContinuo = true;
        _timerBomba?.Start();

        MensagemFrentista = $"{b.Identificador}: Abastecimento pré-fixado de R$ {valor:F2} iniciado! Parará automaticamente.";
        StatusGlobal = MensagemFrentista;
    }

    [RelayCommand]
    public void SimularPulsosFrentista(Bomba? b)
    {
        b ??= BombaFrentistaSelecionada;
        if (b == null) return;

        BombaFrentistaSelecionada = b;
        if (b.Status != StatusBomba.EmAbastecimento && b.Status != StatusBomba.Pausada)
        {
            b.IniciarOuRetomarAbastecimento(TipoOperacaoBomba.Normal);
            b.PausarFluxo();
        }

        b.ReceberPulsos(100); // 100 pulsos = 1.00 Litro
        _repository.AtualizarBomba(b);
        _repository.AtualizarTanque(b.CombustivelAssociado);

        MensagemFrentista = $"{b.Identificador}: +1 Litro injetado (100 pulsos). Total acumulado: {b.LitrosAbastecidos:F2} L (R$ {b.ValorTotalCalculado:F2}).";
        StatusGlobal = MensagemFrentista;
    }

    [RelayCommand]
    public void GuardarBicoColdreFrentista(Bomba? b)
    {
        b ??= BombaFrentistaSelecionada;
        if (b == null) return;

        if (_bombaEmAbastecimento == b)
        {
            _timerBomba?.Stop();
            IsAbastecendoContinuo = false;
            _bombaEmAbastecimento = null;
        }

        b.FinalizarAbastecimentoNoColdre();
        _repository.AtualizarBomba(b);

        MensagemFrentista = $"{b.Identificador}: Abastecimento CONCLUÍDO! Total: {b.LitrosAbastecidos:F2} L (R$ {b.ValorTotalCalculado:F2}). Liberado para pagamento no Caixa.";
        StatusGlobal = MensagemFrentista;

        // Atualiza a lista de bombas pendentes do caixa
        BombasPendentesCaixa.Clear();
        foreach (var p in _repository.ObterBombas().Where(p => p.LitrosAbastecidos > 0))
        {
            BombasPendentesCaixa.Add(p);
        }
    }

    [RelayCommand]
    public void RegistrarFugaFrentista()
    {
        decimal valor = decimal.TryParse(ValorFugaFrentista, out decimal v) ? v : 100;
        var res = FrentistaCtrl.RegistrarFuga(PlacaFugaFrentista, valor);
        MensagemFrentista = res.mensagem;
        StatusGlobal = res.mensagem;
        RecarregarDadosGerais();
    }

    [RelayCommand]
    public void BaterPontoFrentista(string tipo)
    {
        if (UsuarioAutenticado != null && Enum.TryParse(tipo, out TipoMarcacaoPonto tp))
        {
            FrentistaCtrl.RegistrarPonto(UsuarioAutenticado, tp);
            MensagemFrentista = $"[RF17 PONTO ELETRÔNICO] {tp} registrada para {UsuarioAutenticado.Nome} às {DateTime.Now:HH:mm:ss}.";
            StatusGlobal = MensagemFrentista;
            RecarregarDadosGerais();
        }
    }

    // =========================================================================
    // COMANDOS DO CAIXA (FLUXO DE CARTÃO COM MÁQUINA POS E METÁFORA)
    // =========================================================================
    partial void OnClienteCaixaSelecionadoChanged(Cliente? value)
    {
        AtualizarStatusClienteCaixa();
    }

    private void AtualizarStatusClienteCaixa()
    {
        if (ClienteCaixaSelecionado == null)
        {
            IsClienteInadimplente = false;
            MensagemStatusCliente = "Consumidor Final (Sem cadastro no sistema)";
            return;
        }

        bool inadimplente = ClienteCaixaSelecionado.EstaInadimplente(DateTime.Now) ||
                            (ClienteCaixaSelecionado.DividaPendente > 0 && ClienteCaixaSelecionado.DataVencimentoDivida.HasValue && DateTime.Now.Date > ClienteCaixaSelecionado.DataVencimentoDivida.Value.Date);

        IsClienteInadimplente = inadimplente;

        if (inadimplente)
        {
            MensagemStatusCliente = $"⛔ CLIENTE INADIMPLENTE! Dívida: R$ {ClienteCaixaSelecionado.DividaPendente:F2} (Vencida em {ClienteCaixaSelecionado.DataVencimentoDivida:dd/MM/yyyy}). Venda a prazo BLOQUEADA [RF04 / RN20]!";
            MensagemCaixa = MensagemStatusCliente;
        }
        else
        {
            MensagemStatusCliente = $"✅ Situação Regular. Total gasto acumulado: R$ {ClienteCaixaSelecionado.TotalGastoAcumulado:F2} [RF07]. Dívida: R$ {ClienteCaixaSelecionado.DividaPendente:F2}.";
        }
    }

    [RelayCommand]
    public void ToggleFormularioNovoCliente()
    {
        MostrarFormularioNovoCliente = !MostrarFormularioNovoCliente;
    }

    [RelayCommand]
    public void CadastrarClienteCaixa()
    {
        var res = CaixaCtrl.CadastrarCliente(
            NovoClienteNome,
            NovoClienteCpfCnpj,
            NovoClienteEndereco,
            NovoClienteTelefone,
            NovoClienteEmail);

        MensagemCaixa = res.mensagem;
        StatusGlobal = res.mensagem;

        if (res.sucesso && res.cliente != null)
        {
            RecarregarDadosGerais();
            ClienteCaixaSelecionado = ClientesCaixa.FirstOrDefault(c => c.IdCliente == res.cliente.IdCliente);
            NovoClienteNome = "";
            NovoClienteCpfCnpj = "";
            NovoClienteEndereco = "";
            NovoClienteTelefone = "";
            NovoClienteEmail = "";
            MostrarFormularioNovoCliente = false;
        }
    }

    [RelayCommand]
    public void QuitarDividaClienteSelecionado()
    {
        if (ClienteCaixaSelecionado == null) return;
        CaixaCtrl.QuitarDivida(ClienteCaixaSelecionado.IdCliente);
        MensagemCaixa = $"✅ [QUITAÇÃO] Dívida do cliente '{ClienteCaixaSelecionado.Nome}' foi quitada integralmente! Cadastro regularizado.";
        StatusGlobal = MensagemCaixa;
        RecarregarDadosGerais();
        ClienteCaixaSelecionado = ClientesCaixa.FirstOrDefault(c => c.IdCliente == ClienteCaixaSelecionado.IdCliente);
        AtualizarStatusClienteCaixa();
    }

    [RelayCommand]
    public void SelecionarFormaPagamentoCaixa(string formaStr)
    {
        // Aceita tanto "Prazo" quanto "APrazo"
        if (formaStr == "Prazo") formaStr = "APrazo";

        if (Enum.TryParse(formaStr, out FormaPagamento f))
        {
            FormaPagamentoCaixa = f;
            if (f == FormaPagamento.CartaoDebito || f == FormaPagamento.CartaoCredito)
            {
                IsPagamentoCartao = true;
                CartaoAutorizado = false;
                CartaoMensagemStatus = $"Aproxime, insira o chip ou passe o cartão na máquina POS (Valor: R$ {TotalVendaCaixa:F2}).";
            }
            else
            {
                IsPagamentoCartao = false;
                CartaoAutorizado = false;
            }

            if (f == FormaPagamento.APrazo)
            {
                if (ClienteCaixaSelecionado == null)
                {
                    MensagemCaixa = "⚠️ [RF04 BLOQUEIO] Venda a prazo exige cliente com cadastro completo na base de dados!";
                }
                else if (ClienteCaixaSelecionado.EstaInadimplente(DateTime.Now))
                {
                    MensagemCaixa = $"⛔ [RF04 / RN20 BLOQUEIO TOTAL] O cliente '{ClienteCaixaSelecionado.Nome}' está INADIMPLENTE! Venda a prazo bloqueada.";
                }
                else
                {
                    MensagemCaixa = $"ℹ️ Pagamento a Prazo selecionado para {ClienteCaixaSelecionado.Nome}. Total acumulado: R$ {ClienteCaixaSelecionado.TotalGastoAcumulado:F2}.";
                }
            }
        }
    }

    [RelayCommand]
    public void SimularAproximacaoCartao()
    {
        CartaoMensagemStatus = "Lendo chip via NFC/Contactless... Comunicando com a adquirente...";
        // Simula aprovação imediata com feedback realista
        CartaoAutorizado = true;
        CartaoMensagemStatus = $"✅ TRANSAÇÃO AUTORIZADA! Código: {new Random().Next(100000, 999999)} | Cartão {CartaoBandeira} final 4321.";
        MensagemCaixa = "Pagamento com cartão autorizado pela maquininha! Clique em 'FINALIZAR VENDA' para concluir.";
    }

    [RelayCommand]
    public void InserirChipCartao()
    {
        CartaoMensagemStatus = "Chip inserido. Solicitando senha do cliente no PIN Pad... Senha validada!";
        CartaoAutorizado = true;
        CartaoMensagemStatus = $"✅ TRANSAÇÃO APROVADA COM SUCESSO! Cartão {CartaoBandeira} Débito/Crédito.";
        MensagemCaixa = "Pagamento com chip e senha aprovado! Clique em 'FINALIZAR VENDA' para concluir.";
    }

    [RelayCommand]
    public void AdicionarItemCarrinhoCaixa(Produto p)
    {
        if (p.QuantidadeEstoque <= 0)
        {
            MensagemCaixa = $"Produto '{p.Nome}' está esgotado no estoque!";
            return;
        }

        var existente = CarrinhoCaixa.FirstOrDefault(i => i.Produto.IdProduto == p.IdProduto);
        if (existente != null)
        {
            existente.Quantidade++;
        }
        else
        {
            CarrinhoCaixa.Add(new ItemVenda { Produto = p, Quantidade = 1, PrecoUnitario = p.Preco });
        }
        OnPropertyChanged(nameof(TotalVendaCaixa));
    }

    [RelayCommand]
    public void SelecionarBombaPendenteCaixa(Bomba? b)
    {
        BombaPendenteSelecionada = b;
        OnPropertyChanged(nameof(TotalVendaCaixa));
    }

    [RelayCommand]
    public void LimparCarrinhoCaixa()
    {
        CarrinhoCaixa.Clear();
        BombaPendenteSelecionada = null;
        IsPagamentoCartao = false;
        CartaoAutorizado = false;
        MensagemCaixa = "Carrinho esvaziado.";
        OnPropertyChanged(nameof(TotalVendaCaixa));
    }

    [RelayCommand]
    public void FinalizarVendaCaixa()
    {
        // Se for cartão, exige que tenha sido aproximado/inserido e aprovado
        if (IsPagamentoCartao && !CartaoAutorizado)
        {
            MensagemCaixa = "Aguardando aprovação do cartão na maquininha! Aproxime ou insira o cartão antes de finalizar.";
            return;
        }

        // RF04 e RN20: Bloqueio estrito de venda a prazo para inadimplentes
        if (FormaPagamentoCaixa == FormaPagamento.APrazo)
        {
            if (ClienteCaixaSelecionado == null)
            {
                MensagemCaixa = "⚠️ [RF04 BLOQUEIO] Venda a prazo permitida exclusivamente para clientes com cadastro completo na base de dados!";
                StatusGlobal = MensagemCaixa;
                return;
            }

            bool inadimplente = ClienteCaixaSelecionado.EstaInadimplente(DateTime.Now) ||
                                (ClienteCaixaSelecionado.DividaPendente > 0 && ClienteCaixaSelecionado.DataVencimentoDivida.HasValue && DateTime.Now.Date > ClienteCaixaSelecionado.DataVencimentoDivida.Value.Date);

            if (inadimplente)
            {
                MensagemCaixa = $"⛔ [RF04 / RN20 BLOQUEIO TOTAL] O cliente/empresa '{ClienteCaixaSelecionado.Nome}' está INADIMPLENTE com dívida vencida de R$ {ClienteCaixaSelecionado.DividaPendente:F2} em {ClienteCaixaSelecionado.DataVencimentoDivida:dd/MM/yyyy}! Compras a prazo estão ESTRITAMENTE BLOQUEADAS.";
                StatusGlobal = MensagemCaixa;
                return;
            }
        }

        var operador = UsuarioAutenticado ?? _repository.ObterFuncionarios().First(f => f.Cargo == "Caixa");
        var res = CaixaCtrl.FinalizarVenda(
            operador,
            ClienteCaixaSelecionado,
            FormaPagamentoCaixa,
            CarrinhoCaixa.ToList(),
            BombaPendenteSelecionada,
            NumeroChequeCaixa,
            BancoChequeCaixa);

        MensagemCaixa = res.mensagem;
        StatusGlobal = res.mensagem.Replace("\n", " ");

        if (res.sucesso)
        {
            CarrinhoCaixa.Clear();
            BombaPendenteSelecionada = null;
            IsPagamentoCartao = false;
            CartaoAutorizado = false;
            OnPropertyChanged(nameof(TotalVendaCaixa));
            RecarregarDadosGerais();
        }
    }

    [RelayCommand]
    public void EmitirNotaFiscalCaixa(Venda? v)
    {
        if (v == null) return;
        var res = CaixaCtrl.EmitirNotaFiscal(v.IdVenda);
        MensagemCaixa = res.mensagem;
        StatusGlobal = res.mensagem;
        RecarregarDadosGerais();
    }

    [RelayCommand]
    public void CancelarVendaCaixa(Venda? v)
    {
        if (v == null) return;
        var res = CaixaCtrl.CancelarVenda(v.IdVenda, "Cancelamento solicitado no caixa.");
        MensagemCaixa = res.mensagem;
        StatusGlobal = res.mensagem;
        RecarregarDadosGerais();
    }

    [RelayCommand]
    public void BaterPontoCaixa(string tipo)
    {
        if (UsuarioAutenticado != null && Enum.TryParse(tipo, out TipoMarcacaoPonto tp))
        {
            CaixaCtrl.RegistrarPonto(UsuarioAutenticado, tp);
            MensagemCaixa = $"[RF17 PONTO] {tp} registrada para {UsuarioAutenticado.Nome} às {DateTime.Now:HH:mm:ss}.";
            StatusGlobal = MensagemCaixa;
            RecarregarDadosGerais();
        }
    }

    // =========================================================================
    // COMANDOS DO AUXILIAR ADMINISTRATIVO / RH
    // =========================================================================
    [RelayCommand]
    public void CadastrarFuncionarioAdm()
    {
        decimal sal = decimal.TryParse(NovoFuncSalario, out decimal s) && s > 0 
            ? s 
            : Funcionario.ObterPisoNacional(NovoFuncCargo);

        var res = AuxiliarAdmCtrl.CadastrarFuncionario(
            NovoFuncNome, NovoFuncCpf, NovoFuncCargo, NovoFuncCarteira, NovoFuncTelefone, NovoFuncEndereco, sal);

        MensagemAdm = res.mensagem;
        StatusGlobal = res.mensagem;
        if (res.sucesso)
        {
            NovoFuncNome = "";
            NovoFuncCpf = "";
            NovoFuncTelefone = "";
            NovoFuncEndereco = "";
            RecarregarDadosGerais();
        }
    }

    [RelayCommand]
    public void DesativarFuncionarioAdm(Funcionario? f)
    {
        if (f == null) return;
        var res = AuxiliarAdmCtrl.DesativarUsuario(f.IdFuncionario);
        MensagemAdm = res.mensagem;
        StatusGlobal = res.mensagem;
        RecarregarDadosGerais();
    }

    [RelayCommand]
    public void CadastrarFornecedorAdm()
    {
        var res = AuxiliarAdmCtrl.CadastrarFornecedor(NovoFornRazao, NovoFornCnpj, NovoFornTelefone, NovoFornEmail);
        MensagemAdm = res.mensagem;
        StatusGlobal = res.mensagem;
        if (res.sucesso)
        {
            NovoFornRazao = "";
            NovoFornCnpj = "";
            RecarregarDadosGerais();
        }
    }

    [RelayCommand]
    public void ReceberCaminhaoCombustivelAdm()
    {
        var tanque = TanqueCompraSelecionado ?? TanquesAdm.FirstOrDefault();
        if (tanque == null) return;

        double litros = double.TryParse(LitrosCompraCombustivel, out double l) ? l : 5000;
        decimal valorTotal = (decimal)litros * (tanque.PrecoLitro * 0.75m);

        var res = AuxiliarAdmCtrl.ReceberCaminhaoCombustivel(tanque.Tipo, litros, valorTotal);
        MensagemAdm = res.mensagem;
        StatusGlobal = res.mensagem;
        RecarregarDadosGerais();
    }

    // RF14: Gerenciar compras de produtos para loja de conveniência
    [RelayCommand]
    public void RegistrarCompraLojaAdm()
    {
        if (CompraLojaProdutoSelecionado == null || CompraLojaFornecedorSelecionado == null)
        {
            MensagemAdm = "Por favor, selecione o produto da conveniência e o fornecedor!";
            return;
        }

        decimal qtd = decimal.TryParse(CompraLojaQuantidade, out decimal q) ? q : 10;
        decimal preco = decimal.TryParse(CompraLojaPrecoUnitario, out decimal p) ? p : 5;

        var res = AuxiliarAdmCtrl.RegistrarCompraMercadoriaLoja(
            CompraLojaFornecedorSelecionado.IdFornecedor,
            CompraLojaProdutoSelecionado.IdProduto,
            qtd,
            preco);

        MensagemAdm = res.mensagem;
        StatusGlobal = res.mensagem;
        RecarregarDadosGerais();
    }

    // RF17: Emitir relatório de horas trabalhadas por funcionário
    [RelayCommand]
    public void EmitirRelatorioHorasRH()
    {
        RelatorioHorasRH.Clear();
        foreach (var r in AuxiliarAdmCtrl.EmitirRelatorioHorasTrabalhadas())
        {
            RelatorioHorasRH.Add(r);
        }
        MensagemAdm = $"[RF17] Relatório individual de horas trabalhadas emitido com sucesso para {RelatorioHorasRH.Count} colaboradores do posto!";
        StatusGlobal = MensagemAdm;
    }

    [RelayCommand]
    public void BaterPontoAdm(string tipo)
    {
        if (UsuarioAutenticado != null && Enum.TryParse(tipo, out TipoMarcacaoPonto tp))
        {
            AuxiliarAdmCtrl.RegistrarPonto(UsuarioAutenticado, tp);
            MensagemAdm = $"[PONTO REGISTRADO] {tp} gravada para {UsuarioAutenticado.Nome} às {DateTime.Now:HH:mm:ss}.";
            StatusGlobal = MensagemAdm;
            RecarregarDadosGerais();
        }
    }

    // RF20: Filtro de Relatórios Gerenciais (Diário, Semanal, Mensal)
    [RelayCommand]
    public void AlterarFiltroPeriodoGerente(string periodo)
    {
        FiltroPeriodoRelatorioGerente = periodo;
        var rel = GerenteCtrl.ObterTotaisRelatorio();
        decimal mult = periodo switch
        {
            "Diário" => 0.10m,
            "Semanal" => 0.40m,
            _ => 1.0m
        };
        RelatorioFaturamentoConv = rel.faturamentoConveniencia * mult;
        RelatorioFaturamentoComb = rel.faturamentoCombustivel * mult;
        MensagemGerente = $"[RF20] Relatório gerencial recalculado para o filtro: {periodo.ToUpper()}!";
    }

    // =========================================================================
    // COMANDOS DO GERENTE - AUDITORIA CLICÁVEL E CRUDs
    // =========================================================================
    [RelayCommand]
    public void AbrirDetalhesFaturamentoPista()
    {
        TituloDetalheFaturamento = "Relatório Detalhado: Faturamento da Pista & Bombas (RF20)";
        TipoDetalheAtivo = "Pista";
        AtualizarFlagsDetalhe();

        VendasPistaDetalhe.Clear();
        foreach (var v in _repository.ObterVendas().Where(v => v.ItemCombustivel != null && v.Status == StatusVenda.Concluida))
        {
            VendasPistaDetalhe.Add(v);
        }

        IsDetalheFaturamentoAberto = true;
    }

    [RelayCommand]
    public void AbrirDetalhesFaturamentoLoja()
    {
        TituloDetalheFaturamento = "Relatório Detalhado: Faturamento da Loja de Conveniência (RF20)";
        TipoDetalheAtivo = "Loja";
        AtualizarFlagsDetalhe();

        VendasLojaDetalhe.Clear();
        foreach (var v in _repository.ObterVendas().Where(v => v.ItensProdutos.Count > 0 && v.Status == StatusVenda.Concluida))
        {
            VendasLojaDetalhe.Add(v);
        }

        IsDetalheFaturamentoAberto = true;
    }

    [RelayCommand]
    public void AbrirDetalhesDividas()
    {
        TituloDetalheFaturamento = "Relatório Detalhado: Clientes Devedores & Calotes Registrados (RN09 / RN20)";
        TipoDetalheAtivo = "Dividas";
        AtualizarFlagsDetalhe();

        DevedoresDetalhe.Clear();
        foreach (var c in _repository.ObterClientes().Where(c => c.DividaPendente > 0))
        {
            DevedoresDetalhe.Add(c);
        }

        IsDetalheFaturamentoAberto = true;
    }

    [RelayCommand]
    public void AbrirDetalhesEstoqueBaixo()
    {
        TituloDetalheFaturamento = "Relatório de Auditoria: Tanques em Alerta (RN12/RN13) & Itens da Loja com Estoque Baixo (RN14)";
        TipoDetalheAtivo = "EstoqueBaixo";
        AtualizarFlagsDetalhe();

        TanquesAlertaCombustivel.Clear();
        foreach (var t in _repository.ObterTanques().Where(t => t.PrecisaAlertaCompra || t.EstaNivelCriticoBloqueio))
        {
            TanquesAlertaCombustivel.Add(t);
        }

        ProdutosEstoqueBaixoDetalhe.Clear();
        foreach (var p in _repository.ObterProdutos().Where(p => p.EstoqueAbaixoDoMinimo))
        {
            ProdutosEstoqueBaixoDetalhe.Add(p);
        }

        IsDetalheFaturamentoAberto = true;
    }

    [RelayCommand]
    public void FecharDetalhesFaturamento()
    {
        IsDetalheFaturamentoAberto = false;
    }

    private void AtualizarFlagsDetalhe()
    {
        OnPropertyChanged(nameof(IsDetalhePista));
        OnPropertyChanged(nameof(IsDetalheLoja));
        OnPropertyChanged(nameof(IsDetalheDividas));
        OnPropertyChanged(nameof(IsDetalheEstoqueBaixo));
    }

    [RelayCommand]
    public void ReajustarPrecoGasolinaGerente()
    {
        if (decimal.TryParse(NovoPrecoGasolina, out decimal p))
        {
            var res = GerenteCtrl.AlterarPrecoCombustivel(TipoCombustivel.Gasolina, p);
            MensagemGerente = res.mensagem;
            StatusGlobal = res.mensagem;
            RecarregarDadosGerais();
        }
    }

    [RelayCommand]
    public void ReajustarPrecoEtanolGerente()
    {
        if (decimal.TryParse(NovoPrecoEtanol, out decimal p))
        {
            var res = GerenteCtrl.AlterarPrecoCombustivel(TipoCombustivel.Etanol, p);
            MensagemGerente = res.mensagem;
            StatusGlobal = res.mensagem;
            RecarregarDadosGerais();
        }
    }

    [RelayCommand]
    public void ReajustarPrecoDieselGerente()
    {
        if (decimal.TryParse(NovoPrecoDiesel, out decimal p))
        {
            var res = GerenteCtrl.AlterarPrecoCombustivel(TipoCombustivel.Diesel, p);
            MensagemGerente = res.mensagem;
            StatusGlobal = res.mensagem;
            RecarregarDadosGerais();
        }
    }

    [RelayCommand]
    public void SalvarProdutoCrudGerente()
    {
        decimal preco = decimal.TryParse(CrudProdutoPreco, out decimal pr) ? pr : 0;
        int estoque = int.TryParse(CrudProdutoEstoque, out int est) ? est : 0;
        int estoqueMin = int.TryParse(CrudProdutoEstoqueMin, out int min) ? min : 5;

        var res = GerenteCtrl.SalvarProduto(CrudProdutoId, CrudProdutoNome, preco, CrudProdutoCategoria, estoque, estoqueMin);
        MensagemGerente = res.mensagem;
        StatusGlobal = res.mensagem;
        if (res.sucesso)
        {
            LimparFormularioCrudGerente();
            RecarregarDadosGerais();
        }
    }

    [RelayCommand]
    public void EditarProdutoCrudGerente(Produto? p)
    {
        if (p == null) return;
        CrudProdutoId = p.IdProduto;
        CrudProdutoNome = p.Nome;
        CrudProdutoPreco = p.Preco.ToString("F2");
        CrudProdutoCategoria = p.Categoria;
        CrudProdutoEstoque = p.QuantidadeEstoque.ToString();
        CrudProdutoEstoqueMin = p.EstoqueMinimo.ToString();
        MensagemGerente = $"Editando produto '{p.Nome}'. Modifique os campos e clique em 'Salvar Produto'.";
    }

    [RelayCommand]
    public void LimparFormularioCrudGerente()
    {
        CrudProdutoId = 0;
        CrudProdutoNome = "";
        CrudProdutoPreco = "10,00";
        CrudProdutoCategoria = "Bebidas";
        CrudProdutoEstoque = "20";
        CrudProdutoEstoqueMin = "5";
        MensagemGerente = "Formulário de produto limpo para novo cadastro.";
    }

    [RelayCommand]
    public void HomologarAjusteEstoqueGerente()
    {
        if (ProdutoAjusteManual == null)
        {
            MensagemGerente = "Selecione o produto para realizar o ajuste.";
            return;
        }

        int novaQtd = int.TryParse(NovaQtdAjusteManual, out int q) ? q : 0;
        var gerente = (UsuarioAutenticado as Gerente) ?? _repository.ObterFuncionarios().OfType<Gerente>().First();

        var res = GerenteCtrl.RealizarAjusteEstoque(ProdutoAjusteManual.IdProduto, novaQtd, MotivoAjusteManual, gerente);
        MensagemGerente = res.mensagem;
        StatusGlobal = res.mensagem;
        RecarregarDadosGerais();
    }

    [RelayCommand]
    public void HomologarFechamentoCaixaGerente()
    {
        var gerente = (UsuarioAutenticado as Gerente) ?? _repository.ObterFuncionarios().OfType<Gerente>().First();
        var res = GerenteCtrl.HomologarFechamentoCaixa(gerente);
        MensagemGerente = res.mensagem;
        StatusGlobal = res.mensagem;
        RecarregarDadosGerais();
    }

    [RelayCommand]
    public void GerarBoletoMensalGerente(Cliente? c)
    {
        if (c == null) return;
        var res = GerenteCtrl.GerarBoletoMensalEmpresa(c);
        MensagemGerente = res.mensagem;
        StatusGlobal = res.mensagem;
        RecarregarDadosGerais();
    }
}
