using PostoOasis.Core.Interfaces;
using PostoOasis.Core.Models;
using PostoOasis.Core.Services;

namespace PostoOasis.App.Controllers;

public class AuxiliarAdmController
{
    private readonly IPostoRepository _repository;

    public AuxiliarAdmController(IPostoRepository repository)
    {
        _repository = repository;
    }

    public IReadOnlyList<Funcionario> ObterFuncionarios() => _repository.ObterFuncionarios();
    public IReadOnlyList<Fornecedor> ObterFornecedores() => _repository.ObterFornecedores();
    public IReadOnlyList<Combustivel> ObterTanques() => _repository.ObterTanques();
    public IReadOnlyList<Produto> ObterProdutos() => _repository.ObterProdutos();

    // UC15 / RF15 / RN04 / RN05: Cadastrar funcionário com validações
    public (bool sucesso, string mensagem) CadastrarFuncionario(
        string nome, string cpf, string cargo, string carteira, string telefone, string endereco, decimal salario)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return (false, "Por favor, informe o nome completo do colaborador!");

        // Remove pontuação (pontos e traços)
        string cpfLimpo = new string((cpf ?? "").Where(char.IsDigit).ToArray());

        // Para facilitar os testes em sala de aula, aceita qualquer sequência de 11 dígitos
        if (cpfLimpo.Length != 11)
            return (false, "O CPF deve conter exatamente 11 dígitos numéricos (ex: 11122233344 ou 123.456.789-00)!");

        // Validação de unicidade no posto: não pode repetir mesmo CPF já cadastrado
        bool cpfDuplicado = _repository.ObterFuncionarios()
            .Any(f => new string(f.Cpf.Where(char.IsDigit).ToArray()) == cpfLimpo);

        if (cpfDuplicado)
            return (false, $"[CPF JÁ CADASTRADO] O CPF '{cpf}' já pertence a outro colaborador cadastrado no posto!");

        // RN05: Salário não pode ser inferior ao piso nacional da função
        decimal piso = Funcionario.ObterPisoNacional(cargo);
        if (salario < piso)
            return (false, $"[RN05 PISO SALARIAL] O salário proposto (R$ {salario:F2}) é inferior ao piso nacional da categoria de {cargo} (R$ {piso:F2})!");

        var novoFunc = new Funcionario
        {
            Nome = nome.Trim(),
            Cpf = cpf.Trim(),
            Cargo = cargo.Trim(),
            CarteiraTrabalho = string.IsNullOrWhiteSpace(carteira) ? $"CTPS-{cpfLimpo.Substring(0, 5)}" : carteira.Trim(),
            Telefone = string.IsNullOrWhiteSpace(telefone) ? "(44) 99888-7766" : telefone.Trim(),
            Endereco = string.IsNullOrWhiteSpace(endereco) ? "Maringá - PR" : endereco.Trim(),
            Salario = salario,
            DataAdmissao = DateTime.Now,
            Usuario = nome.Trim().ToLowerInvariant().Replace(" ", ".") + ".oasis",
            Senha = "123",
            Ativo = true
        };

        _repository.SalvarFuncionario(novoFunc);
        return (true, $"[SUCESSO] Colaborador '{novoFunc.Nome}' admitido com sucesso! CPF: {novoFunc.Cpf}. Usuário gerado: {novoFunc.Usuario} (Senha: 123).");
    }

    // RF18 / UC18: Desativar usuário desligado
    public (bool sucesso, string mensagem) DesativarUsuario(int idFuncionario)
    {
        var func = _repository.ObterFuncionarios().FirstOrDefault(f => f.IdFuncionario == idFuncionario);
        if (func == null) return (false, "Colaborador não encontrado.");

        _repository.DesativarFuncionario(idFuncionario);
        return (true, $"[RF18 DESATIVAÇÃO] Colaborador '{func.Nome}' desativado com sucesso. Acesso bloqueado.");
    }

    // RF13 / RN08: Cadastrar fornecedor da conveniência
    public (bool sucesso, string mensagem) CadastrarFornecedor(string razaoSocial, string cnpj, string telefone, string? email = null)
    {
        if (string.IsNullOrWhiteSpace(razaoSocial))
            return (false, "Informe a razão social da distribuidora!");

        string cnpjLimpo = new string((cnpj ?? "").Where(char.IsDigit).ToArray());
        if (cnpjLimpo.Length != 14)
            return (false, "O CNPJ do fornecedor deve conter 14 dígitos numéricos!");

        bool cnpjDuplicado = _repository.ObterFornecedores()
            .Any(fn => new string(fn.Cnpj.Where(char.IsDigit).ToArray()) == cnpjLimpo);

        if (cnpjDuplicado)
            return (false, $"[CNPJ DUPLICADO] Já existe um fornecedor cadastrado com o CNPJ '{cnpj}'!");

        var novoForn = new Fornecedor
        {
            RazaoSocial = razaoSocial.Trim(),
            Cnpj = cnpj.Trim(),
            Telefone = string.IsNullOrWhiteSpace(telefone) ? "(44) 3333-2222" : telefone.Trim()
        };

        _repository.SalvarFornecedor(novoForn);
        return (true, $"[RF13 SUCESSO] Distribuidora '{razaoSocial}' cadastrada com sucesso!");
    }

    // RF09 / RN15: Receber caminhão de combustível (Petrobras exclusiva)
    public (bool sucesso, string mensagem) ReceberCaminhaoCombustivel(TipoCombustivel tipo, double litros, decimal? valorTotal = null)
    {
        if (litros <= 0) return (false, "Quantidade de litros inválida.");

        var tanque = _repository.ObterTanquePorTipo(tipo);
        if (tanque == null) return (false, "Tanque não encontrado.");

        tanque.Reabastecer(litros);
        _repository.AtualizarTanque(tanque);

        // Desbloqueia bombas que estavam bloqueadas por nível crítico caso nível normalize (> 4%)
        foreach (var b in _repository.ObterBombas().Where(b => b.CombustivelAssociado.Tipo == tipo))
        {
            if (b.Status == StatusBomba.BloqueadaSegurancaCritica && !tanque.EstaNivelCriticoBloqueio)
            {
                b.Status = StatusBomba.Disponivel;
                _repository.AtualizarBomba(b);
            }
        }

        return (true, $"[RF09 / RN15 REABASTECIMENTO] Recebidos {litros:F0} Litros da Petrobras para o tanque de {tanque.Nome}. Nível atual: {tanque.NivelAtualLitros:F0}/{tanque.CapacidadeTotalLitros:F0} L ({tanque.PercentualNivel:F1}%).");
    }

    // RF14: Gerenciar compras de produtos para a loja de conveniência
    public (bool sucesso, string mensagem) RegistrarCompraMercadoriaLoja(
        int idFornecedor,
        int idProduto,
        decimal quantidade,
        decimal precoUnitario)
    {
        var fornecedor = _repository.ObterFornecedores().FirstOrDefault(f => f.IdFornecedor == idFornecedor);
        if (fornecedor == null) return (false, "Fornecedor não selecionado!");

        var produto = _repository.ObterProdutos().FirstOrDefault(p => p.IdProduto == idProduto);
        if (produto == null) return (false, "Produto não selecionado!");

        if (quantidade <= 0 || precoUnitario <= 0)
            return (false, "Quantidade e Preço Unitário devem ser maiores que zero!");

        var compra = new CompraMercadoria
        {
            Data = DateTime.Now,
            Fornecedor = fornecedor,
            NomeItem = produto.Nome,
            Quantidade = quantidade,
            PrecoUnitario = precoUnitario,
            Categoria = "Conveniência"
        };

        _repository.RegistrarCompraMercadoria(compra);
        return (true, $"[RF14 ENTRADA DE NOTA] Compra registrada com sucesso! +{quantidade:F0} un de '{produto.Nome}' pelo fornecedor '{fornecedor.RazaoSocial}' (Total: R$ {compra.PrecoTotal:F2}). Estoque atualizado.");
    }

    public IReadOnlyList<CompraMercadoria> ObterComprasMercadorias() => _repository.ObterComprasMercadorias();

    // RF17: Emitir relatório de horas trabalhadas por funcionário
    public IReadOnlyList<RelatorioHorasFuncionario> EmitirRelatorioHorasTrabalhadas() => _repository.GerarRelatorioHorasTrabalhadas();

    public void RegistrarPonto(Funcionario f, TipoMarcacaoPonto tipo) => _repository.RegistrarPonto(f, tipo);
}
