using PostoOasis.Core.Interfaces;
using PostoOasis.Core.Models;

namespace PostoOasis.App.Controllers;

public class FrentistaController
{
    private readonly IPostoRepository _repository;

    public FrentistaController(IPostoRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<Bomba> ObterBombas() => _repository.ObterBombas();

    public (bool sucesso, string mensagem) LiberarBomba(int idBomba, TipoOperacaoBomba modo, decimal? valorPredeterminado = null)
    {
        var bomba = _repository.ObterBombaPorId(idBomba);
        if (bomba == null) return (false, "Bomba não encontrada.");

        // RN12: Tanque não pode ficar abaixo de 4% (bloqueia bomba)
        if (bomba.CombustivelAssociado.EstaNivelCriticoBloqueio)
        {
            bomba.Status = StatusBomba.BloqueadaSegurancaCritica;
            _repository.AtualizarBomba(bomba);
            return (false, $"[RN12 BLOQUEIO] {bomba.Identificador} bloqueada! Nível do tanque de {bomba.CombustivelAssociado.Nome} crítico ({bomba.CombustivelAssociado.PercentualNivel:F1}% <= 4%).");
        }

        bomba.IniciarOuRetomarAbastecimento(modo, valorPredeterminado);
        _repository.AtualizarBomba(bomba);
        return (true, $"{bomba.Identificador} liberada para abastecimento ({modo}).");
    }

    public (bool sucesso, string mensagem) PausarBomba(int idBomba)
    {
        var bomba = _repository.ObterBombaPorId(idBomba);
        if (bomba == null) return (false, "Bomba não encontrada.");

        bomba.PausarFluxo();
        _repository.AtualizarBomba(bomba);
        return (true, $"{bomba.Identificador}: Fluxo pausado em {bomba.LitrosAbastecidos:F2} L.");
    }

    public (bool sucesso, string mensagem) SimularPulsos(int idBomba, int quantidadePulsos)
    {
        var bomba = _repository.ObterBombaPorId(idBomba);
        if (bomba == null || bomba.Status != StatusBomba.EmAbastecimento)
            return (false, "Bomba não está em abastecimento.");

        // RF01: Cada pulso registra 0,01 litro
        bomba.ReceberPulsos(quantidadePulsos);

        _repository.AtualizarBomba(bomba);
        _repository.AtualizarTanque(bomba.CombustivelAssociado);

        return (true, $"{bomba.Identificador}: {bomba.LitrosAbastecidos:F2} L - Total: R$ {bomba.ValorTotalCalculado:F2}");
    }

    public (bool sucesso, string mensagem) GuardarBicoNoColdre(int idBomba)
    {
        var bomba = _repository.ObterBombaPorId(idBomba);
        if (bomba == null) return (false, "Bomba não encontrada.");

        bomba.FinalizarAbastecimentoNoColdre();
        _repository.AtualizarBomba(bomba);
        return (true, $"{bomba.Identificador}: Bico recolhido ao coldre ({bomba.LitrosAbastecidos:F2} L). Abastecimento concluído e liberado para faturamento no Caixa!");
    }

    public (bool sucesso, string mensagem) RegistrarFuga(string placaOuCpf, decimal valor)
    {
        if (string.IsNullOrWhiteSpace(placaOuCpf) || valor <= 0)
            return (false, "Informe a placa/CPF e o valor do abastecimento.");

        // RN09: Cliente fugiu sem pagar -> Quitar até dia 5 do mês seguinte
        _repository.RegistrarFugaNaoPaga(placaOuCpf, valor, DateTime.Now);
        return (true, $"[RN09 REGISTRO DE FUGA] Débito de R$ {valor:F2} vinculado à placa/documento '{placaOuCpf}'. Vencimento fixado no dia 05 do mês seguinte.");
    }

    public void RegistrarPonto(Funcionario f, TipoMarcacaoPonto tipo) => _repository.RegistrarPonto(f, tipo);
}
