using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PostoOasis.Core.Models;

public enum StatusBomba
{
    Disponivel,
    EmAbastecimento,
    Pausada,
    AguardandoPagamento,
    BloqueadaSegurancaCritica, // RN12: Nível < 4%
    DesativadaNoColdre
}

public enum TipoOperacaoBomba
{
    Normal, // Abastecimento livre até o cliente mandar parar
    EspecialValorFixo // Digita valor predeterminado (ex: R$ 50) e para sozinha
}

public class Bomba : INotifyPropertyChanged
{
    private double _litrosAbastecidos = 0.0;
    private StatusBomba _status = StatusBomba.Disponivel;
    private TipoOperacaoBomba _modoOperacao = TipoOperacaoBomba.Normal;
    private decimal? _limiteValorPredeterminado;
    private bool _bicoNoColdre = true;

    public event PropertyChangedEventHandler? PropertyChanged;

    public void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public int IdBomba { get; set; }
    public string Identificador => $"Bomba #{IdBomba:D2}";
    public Combustivel CombustivelAssociado { get; set; } = null!;

    public StatusBomba Status
    {
        get => _status;
        set
        {
            if (_status != value)
            {
                _status = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TextoStatus));
                OnPropertyChanged(nameof(TextoBotaoFluxo));
                OnPropertyChanged(nameof(IsEmAbastecimento));
                OnPropertyChanged(nameof(IsPausada));
            }
        }
    }

    public string TextoStatus => Status switch
    {
        StatusBomba.Disponivel => "Pronta / Disponível",
        StatusBomba.EmAbastecimento => "⛽ Abastecendo...",
        StatusBomba.Pausada => "⏸️ Pausada (Gatilho Solto)",
        StatusBomba.AguardandoPagamento => "💳 Pronto / No Caixa",
        StatusBomba.BloqueadaSegurancaCritica => "⛔ Bloqueada (Tanque < 4%)",
        _ => Status.ToString()
    };

    public string TextoBotaoFluxo => Status switch
    {
        StatusBomba.EmAbastecimento => "⏸️ Pausar Fluxo",
        StatusBomba.Pausada => "▶️ Retomar Fluxo",
        _ => "▶️ Iniciar Fluxo Livre"
    };

    public bool IsEmAbastecimento => Status == StatusBomba.EmAbastecimento;
    public bool IsPausada => Status == StatusBomba.Pausada;

    public TipoOperacaoBomba ModoOperacao
    {
        get => _modoOperacao;
        set { _modoOperacao = value; OnPropertyChanged(); }
    }

    public double LitrosAbastecidos
    {
        get => _litrosAbastecidos;
        set
        {
            _litrosAbastecidos = Math.Round(value, 2);
            OnPropertyChanged();
            OnPropertyChanged(nameof(ValorTotalCalculado));
            OnPropertyChanged(nameof(LitrosFormatado));
            OnPropertyChanged(nameof(ValorFormatado));
        }
    }

    public decimal ValorTotalCalculado => Math.Round((decimal)LitrosAbastecidos * (CombustivelAssociado?.PrecoLitro ?? 0), 2);
    public string LitrosFormatado => $"{LitrosAbastecidos:F2} L";
    public string ValorFormatado => $"R$ {ValorTotalCalculado:F2}";

    public decimal? LimiteValorPredeterminado
    {
        get => _limiteValorPredeterminado;
        set { _limiteValorPredeterminado = value; OnPropertyChanged(); }
    }

    public bool BicoNoColdre
    {
        get => _bicoNoColdre;
        set { _bicoNoColdre = value; OnPropertyChanged(); }
    }

    // RF01: Cada pulso recebido equivale a 0,01 litro (1/100)
    public void ReceberPulsoAbastecimento()
    {
        ReceberPulsos(1);
    }

    public void ReceberPulsos(int quantidade)
    {
        if (CombustivelAssociado.EstaNivelCriticoBloqueio)
        {
            Status = StatusBomba.BloqueadaSegurancaCritica;
            return;
        }

        if (Status != StatusBomba.EmAbastecimento) return;

        double litros = quantidade * 0.01;
        LitrosAbastecidos += litros;
        CombustivelAssociado.ConsumirLitros(litros);

        // Se for modo especial com valor fixo, interrompe automaticamente
        if (ModoOperacao == TipoOperacaoBomba.EspecialValorFixo && LimiteValorPredeterminado.HasValue)
        {
            if (ValorTotalCalculado >= LimiteValorPredeterminado.Value)
            {
                PausarFluxo();
            }
        }
    }

    public void IniciarOuRetomarAbastecimento(TipoOperacaoBomba modo, decimal? valorFixo = null)
    {
        if (CombustivelAssociado.EstaNivelCriticoBloqueio)
        {
            Status = StatusBomba.BloqueadaSegurancaCritica;
            return;
        }

        ModoOperacao = modo;
        LimiteValorPredeterminado = valorFixo;
        BicoNoColdre = false;

        // Se estava pausada, continua da onde parou! Só zera se estava disponível ou aguardando pagamento
        if (Status != StatusBomba.Pausada && Status != StatusBomba.EmAbastecimento)
        {
            LitrosAbastecidos = 0.0;
        }

        Status = StatusBomba.EmAbastecimento;
    }

    public void PausarFluxo()
    {
        if (Status == StatusBomba.EmAbastecimento)
        {
            Status = StatusBomba.Pausada;
        }
    }

    public void FinalizarAbastecimentoNoColdre()
    {
        Status = StatusBomba.AguardandoPagamento;
        BicoNoColdre = true;
    }

    // Mantido por compatibilidade
    public void IniciarAbastecimento(TipoOperacaoBomba modo, decimal? valorFixo = null) => IniciarOuRetomarAbastecimento(modo, valorFixo);
    public void FinalizarAbastecimento() => FinalizarAbastecimentoNoColdre();
}
