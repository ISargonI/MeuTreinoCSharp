using PostoOasis.Core.Models;

namespace PostoOasis.Core.Interfaces;

public interface IPostoRepository
{
    // Tanques e Combustíveis
    IReadOnlyList<Combustivel> ObterTanques();
    Combustivel? ObterTanquePorTipo(TipoCombustivel tipo);
    void AtualizarTanque(Combustivel tanque);

    // Bombas
    IReadOnlyList<Bomba> ObterBombas();
    Bomba? ObterBombaPorId(int idBomba);
    void AtualizarBomba(Bomba bomba);

    // Produtos e Loja de Conveniência
    IReadOnlyList<Produto> ObterProdutos();
    Produto? ObterProdutoPorId(int idProduto);
    void SalvarProduto(Produto produto);
    void RealizarAjusteEstoque(int idProduto, int novaQuantidade, string motivo, Gerente gerente);

    // Clientes e Fidelidade
    IReadOnlyList<Cliente> ObterClientes();
    Cliente? ObterClientePorDoc(string cpfCnpj);
    void SalvarCliente(Cliente cliente);
    void QuitarDividaCliente(int idCliente);
    void RegistrarFugaNaoPaga(string placaOuCpf, decimal valor, DateTime dataAtual);

    // Compras de Mercadorias e Combustíveis (RF09 / RF14)
    IReadOnlyList<CompraMercadoria> ObterComprasMercadorias();
    void RegistrarCompraMercadoria(CompraMercadoria compra);

    // Relatório de Horas Trabalhadas (RF17)
    IReadOnlyList<RelatorioHorasFuncionario> GerarRelatorioHorasTrabalhadas();

    // Vendas e Notas Fiscais
    IReadOnlyList<Venda> ObterVendas();
    Venda RegistrarVenda(Venda venda);
    bool CancelarVenda(int idVenda, string motivo, DateTime agora);
    NotaFiscal EmitirNotaFiscal(int idVenda);

    // Bônus Lavagem
    IReadOnlyList<BonusLavagem> ObterVouchers();
    BonusLavagem? ValidarVoucher(string codigoVoucher, DateTime dataAtual);

    // Funcionários e RH
    IReadOnlyList<Funcionario> ObterFuncionarios();
    Funcionario? ObterFuncionarioPorUsuario(string usuario, string senha);
    void SalvarFuncionario(Funcionario funcionario);
    void DesativarFuncionario(int idFuncionario);

    // Caixa
    SessaoCaixa ObterSessaoCaixaAtual();
    void FecharSessaoCaixa(Gerente gerenteValidador);

    // Fornecedores
    IReadOnlyList<Fornecedor> ObterFornecedores();
    void SalvarFornecedor(Fornecedor fornecedor);

    // Ponto
    IReadOnlyList<RegistroPonto> ObterRegistrosPonto();
    void RegistrarPonto(Funcionario funcionario, TipoMarcacaoPonto tipo);
}
