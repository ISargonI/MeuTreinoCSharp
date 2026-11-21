using PostoOasis.Core.Interfaces;
using PostoOasis.Core.Models;

namespace PostoOasis.Data.Repositories;

public class PostoRepository : IPostoRepository
{
    private readonly List<Combustivel> _tanques = new();
    private readonly List<Bomba> _bombas = new();
    private readonly List<Produto> _produtos = new();
    private readonly List<Cliente> _clientes = new();
    private readonly List<Funcionario> _funcionarios = new();
    private readonly List<Venda> _vendas = new();
    private readonly List<BonusLavagem> _vouchers = new();
    private readonly List<Fornecedor> _fornecedores = new();
    private readonly List<RegistroPonto> _pontos = new();
    private readonly List<CompraMercadoria> _comprasMercadorias = new();
    private SessaoCaixa _sessaoAtual;

    public PostoRepository()
    {
        InicializarDadosSeed();
        _sessaoAtual = new SessaoCaixa
        {
            IdSessao = 1,
            SaldoAbertura = 500.00m,
            OperadorAbertura = _funcionarios.First(f => f.Cargo == "Caixa"),
            DataAbertura = DateTime.Now.Date.AddHours(6)
        };
    }

    private void InicializarDadosSeed()
    {
        // 1. Tanques de Combustível (3 tanques)
        var tanqueGasolina = new Combustivel
        {
            IdCombustivel = 1,
            Tipo = TipoCombustivel.Gasolina,
            PrecoLitro = 5.89m,
            CapacidadeTotalLitros = 20000.0,
            NivelAtualLitros = 2800.0 // 14% -> Em alerta de compra (RN13)
        };

        var tanqueEtanol = new Combustivel
        {
            IdCombustivel = 2,
            Tipo = TipoCombustivel.Etanol,
            PrecoLitro = 3.99m,
            CapacidadeTotalLitros = 15000.0,
            NivelAtualLitros = 550.0 // 3.66% -> Nível crítico < 4%! Bloqueia bomba (RN12)
        };

        var tanqueDiesel = new Combustivel
        {
            IdCombustivel = 3,
            Tipo = TipoCombustivel.Diesel,
            PrecoLitro = 6.19m,
            CapacidadeTotalLitros = 25000.0,
            NivelAtualLitros = 16000.0 // 64% -> Normal
        };

        _tanques.AddRange(new[] { tanqueGasolina, tanqueEtanol, tanqueDiesel });

        // 2. Bombas (6 bombas ligadas aos tanques)
        _bombas.Add(new Bomba { IdBomba = 1, CombustivelAssociado = tanqueGasolina, Status = StatusBomba.Disponivel });
        _bombas.Add(new Bomba { IdBomba = 2, CombustivelAssociado = tanqueGasolina, Status = StatusBomba.Disponivel });
        _bombas.Add(new Bomba { IdBomba = 3, CombustivelAssociado = tanqueEtanol, Status = StatusBomba.BloqueadaSegurancaCritica }); // Bloqueada por RN12
        _bombas.Add(new Bomba { IdBomba = 4, CombustivelAssociado = tanqueEtanol, Status = StatusBomba.BloqueadaSegurancaCritica });
        _bombas.Add(new Bomba { IdBomba = 5, CombustivelAssociado = tanqueDiesel, Status = StatusBomba.Disponivel });
        _bombas.Add(new Bomba { IdBomba = 6, CombustivelAssociado = tanqueDiesel, Status = StatusBomba.Disponivel });

        // 3. Fornecedores
        _fornecedores.Add(new Fornecedor
        {
            IdFornecedor = 1,
            RazaoSocial = "Petrobrás Distribuidora S.A.",
            Cnpj = "33000167000101",
            Telefone = "(45) 3264-1000",
            Email = "combustiveis@petrobras.com.br",
            ExclusivoCombustivelBandeirado = true // RN15
        });

        _fornecedores.Add(new Fornecedor
        {
            IdFornecedor = 2,
            RazaoSocial = "Distribuidora de Alimentos Oeste Ltda",
            Cnpj = "07526557000100",
            Telefone = "(45) 3264-2200",
            Email = "vendas@alimentosconveniencia.com.br",
            ExclusivoCombustivelBandeirado = false
        });

        // 4. Produtos da Loja de Conveniência
        _produtos.Add(new Produto { IdProduto = 1, Nome = "Café Expresso Especial", Preco = 6.50m, Categoria = "Bebidas", QuantidadeEstoque = 45, EstoqueMinimo = 10 });
        _produtos.Add(new Produto { IdProduto = 2, Nome = "Refrigerante Lata 350ml", Preco = 5.00m, Categoria = "Bebidas", QuantidadeEstoque = 18, EstoqueMinimo = 12 });
        _produtos.Add(new Produto { IdProduto = 3, Nome = "Pão de Queijo Mineiro", Preco = 4.50m, Categoria = "Salgados", QuantidadeEstoque = 22, EstoqueMinimo = 8 });
        _produtos.Add(new Produto { IdProduto = 4, Nome = "Óleo Motor Lubrax 15W40", Preco = 42.00m, Categoria = "Automotivo", QuantidadeEstoque = 2, EstoqueMinimo = 5 }); // RN14: Abaixo do mínimo!
        _produtos.Add(new Produto { IdProduto = 5, Nome = "Água Mineral 500ml", Preco = 3.50m, Categoria = "Bebidas", QuantidadeEstoque = 30, EstoqueMinimo = 10 });
        _produtos.Add(new Produto { IdProduto = 6, Nome = "Barra de Chocolate 90g", Preco = 7.90m, Categoria = "Doces", QuantidadeEstoque = 3, EstoqueMinimo = 6 }); // RN14: Abaixo do mínimo!

        // 5. Funcionários
        var gerente = new Gerente
        {
            IdFuncionario = 1,
            Nome = "Alan Gavioli",
            Cpf = "12345678909",
            Cargo = "Gerente",
            Matricula = "GER-001",
            CarteiraTrabalho = "12345/001",
            Telefone = "(45) 99911-2233",
            Endereco = "Av. Brasil, 1000 - Medianeira",
            Salario = 5500.00m,
            DataAdmissao = new DateTime(2022, 1, 15),
            Usuario = "alan.gerente",
            Senha = "123"
        };

        var frentista = new Funcionario
        {
            IdFuncionario = 2,
            Nome = "Wilson Castaway",
            Cpf = "23456789012",
            Cargo = "Frentista",
            Matricula = "FRT-002",
            CarteiraTrabalho = "23456/002",
            Telefone = "(45) 99922-3344",
            Endereco = "Rua Pará, 250",
            Salario = 1950.00m,
            DataAdmissao = new DateTime(2023, 6, 1),
            Usuario = "wilson.pista",
            Senha = "123"
        };

        var caixa = new Funcionario
        {
            IdFuncionario = 3,
            Nome = "Travis Bickle",
            Cpf = "34567890123",
            Cargo = "Caixa",
            Matricula = "CXA-003",
            CarteiraTrabalho = "34567/003",
            Telefone = "(45) 99933-4455",
            Endereco = "Rua Iguaçu, 88",
            Salario = 1900.00m,
            DataAdmissao = new DateTime(2024, 2, 10),
            Usuario = "travis.caixa",
            Senha = "123"
        };

        var rh = new Funcionario
        {
            IdFuncionario = 4,
            Nome = "R. Raskolnikov",
            Cpf = "45678901234",
            Cargo = "RH",
            Matricula = "RH-004",
            CarteiraTrabalho = "45678/004",
            Telefone = "(45) 99944-5566",
            Endereco = "Rua Riachuelo, 400",
            Salario = 2800.00m,
            DataAdmissao = new DateTime(2023, 3, 20),
            Usuario = "raskolnikov.rh",
            Senha = "123"
        };

        _funcionarios.AddRange(new[] { gerente, frentista, caixa, rh });

        // 6. Clientes
        var clienteFiel = new Cliente
        {
            IdCliente = 1,
            Nome = "João Silva Sauro",
            CpfCnpj = "11144477735",
            Telefone = "(45) 98801-0203",
            Email = "joao.silva@email.com",
            Endereco = "Rua das Flores, 50",
            DataCadastro = DateTime.Now.AddMonths(-6), // Mais de 3 meses -> Elegível para cheque (RN18)
            TotalGastoAcumulado = 380.00m,
            DividaPendente = 0
        };

        var clienteEmpresa = new Cliente
        {
            IdCliente = 2,
            Nome = "Transportadora Rápido Medianeira Ltda",
            CpfCnpj = "12345678000195", // CNPJ -> Recebe cobrança por boleto mensal (RN11)
            Telefone = "(45) 3264-9988",
            Email = "financeiro@rapidomedianeira.com.br",
            Endereco = "Rodovia BR-277, Km 670",
            DataCadastro = DateTime.Now.AddMonths(-12),
            TotalGastoAcumulado = 4500.00m,
            DividaPendente = 1250.00m,
            DataVencimentoDivida = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 5) // Quitação dia 5
        };

        var clienteInadimplente = new Cliente
        {
            IdCliente = 3,
            Nome = "Marcos Caloteiro Fujão",
            CpfCnpj = "55566677788",
            Telefone = "(45) 97711-2233",
            Email = "marcos@email.com",
            Endereco = "Rua Desconhecida, 00",
            DataCadastro = DateTime.Now.AddMonths(-2),
            TotalGastoAcumulado = 120.00m,
            DividaPendente = 220.00m,
            DataVencimentoDivida = DateTime.Now.AddDays(-10), // Vencimento no passado -> Inadimplente bloqueado (RN20)
            PlacaVeiculoRegistroDivida = "ABC-1234"
        };

        _clientes.AddRange(new[] { clienteFiel, clienteEmpresa, clienteInadimplente });

        // 7. Voucher de Lavagem inicial (RN03)
        _vouchers.Add(new BonusLavagem
        {
            IdBonus = 1,
            CodigoVoucher = "LAV-9842",
            Cliente = clienteFiel,
            ValorGastoOrigem = 240.00m,
            DataEmissao = DateTime.Now.AddDays(-5),
            Utilizado = false
        });

        // 8. Vendas Iniciais para Auditoria e Relatórios (RF20 / Pista e Loja)
        var bomba1 = _bombas[0]; // Gasolina
        var bomba5 = _bombas[4]; // Diesel
        var prodCafe = _produtos[0];
        var prodPao = _produtos[1];
        var prodRefri = _produtos[2];

        _vendas.Add(new Venda
        {
            IdVenda = 1,
            DataHora = DateTime.Now.AddHours(-2),
            Operador = caixa,
            Frentista = frentista,
            Cliente = clienteFiel,
            ItemCombustivel = new ItemCombustivel
            {
                Bomba = bomba1,
                Litros = 40.0,
                PrecoLitro = bomba1.CombustivelAssociado.PrecoLitro
            },
            Pagamento = new Pagamento { Forma = FormaPagamento.CartaoDebito, Valor = 40.0m * bomba1.CombustivelAssociado.PrecoLitro }
        });

        _vendas.Add(new Venda
        {
            IdVenda = 2,
            DataHora = DateTime.Now.AddHours(-1.5),
            Operador = caixa,
            Frentista = frentista,
            Cliente = clienteEmpresa,
            ItemCombustivel = new ItemCombustivel
            {
                Bomba = bomba5,
                Litros = 120.0,
                PrecoLitro = bomba5.CombustivelAssociado.PrecoLitro
            },
            Pagamento = new Pagamento { Forma = FormaPagamento.APrazo, Valor = 120.0m * bomba5.CombustivelAssociado.PrecoLitro }
        });

        _vendas.Add(new Venda
        {
            IdVenda = 3,
            DataHora = DateTime.Now.AddMinutes(-45),
            Operador = caixa,
            Cliente = clienteFiel,
            ItensProdutos = new List<ItemVenda>
            {
                new() { Produto = prodCafe, Quantidade = 2, PrecoUnitario = prodCafe.Preco },
                new() { Produto = prodPao, Quantidade = 3, PrecoUnitario = prodPao.Preco }
            },
            Pagamento = new Pagamento { Forma = FormaPagamento.Dinheiro, Valor = (2 * prodCafe.Preco) + (3 * prodPao.Preco) }
        });

        _vendas.Add(new Venda
        {
            IdVenda = 4,
            DataHora = DateTime.Now.AddMinutes(-20),
            Operador = caixa,
            ItensProdutos = new List<ItemVenda>
            {
                new() { Produto = prodRefri, Quantidade = 2, PrecoUnitario = prodRefri.Preco }
            },
            Pagamento = new Pagamento { Forma = FormaPagamento.CartaoCredito, Valor = 2 * prodRefri.Preco }
        });

        // RF09: Compra de Combustível da Petrobrás
        var fornPetrobras = _fornecedores.First(f => f.ExclusivoCombustivelBandeirado);
        _comprasMercadorias.Add(new CompraMercadoria
        {
            IdCompra = 1,
            Data = DateTime.Now.AddDays(-2),
            Fornecedor = fornPetrobras,
            NomeItem = "Gasolina Comum (Caminhão-Tanque Petrobras)",
            Quantidade = 5000,
            PrecoUnitario = 4.10m,
            Categoria = "Combustível"
        });

        // RF14: Compras de Produtos para a Loja de Conveniência
        var fornAlimentos = _fornecedores.First(f => !f.ExclusivoCombustivelBandeirado);
        _comprasMercadorias.Add(new CompraMercadoria
        {
            IdCompra = 2,
            Data = DateTime.Now.AddDays(-1),
            Fornecedor = fornAlimentos,
            NomeItem = "Café Especial em Grãos 1kg",
            Quantidade = 20,
            PrecoUnitario = 22.00m,
            Categoria = "Conveniência"
        });
        _comprasMercadorias.Add(new CompraMercadoria
        {
            IdCompra = 3,
            Data = DateTime.Now.AddHours(-5),
            Fornecedor = fornAlimentos,
            NomeItem = "Refrigerante Lata 350ml (Pack c/ 24)",
            Quantidade = 15,
            PrecoUnitario = 3.20m,
            Categoria = "Conveniência"
        });
    }

    public IReadOnlyList<Combustivel> ObterTanques() => _tanques.AsReadOnly();

    public Combustivel? ObterTanquePorTipo(TipoCombustivel tipo) => _tanques.FirstOrDefault(t => t.Tipo == tipo);

    public void AtualizarTanque(Combustivel tanque)
    {
        var existente = ObterTanquePorTipo(tanque.Tipo);
        if (existente != null)
        {
            existente.NivelAtualLitros = tanque.NivelAtualLitros;
            existente.PrecoLitro = tanque.PrecoLitro;
        }
    }

    public IReadOnlyList<Bomba> ObterBombas() => _bombas.AsReadOnly();

    public Bomba? ObterBombaPorId(int idBomba) => _bombas.FirstOrDefault(b => b.IdBomba == idBomba);

    public void AtualizarBomba(Bomba bomba)
    {
        var existente = ObterBombaPorId(bomba.IdBomba);
        if (existente != null)
        {
            existente.Status = bomba.Status;
            existente.LitrosAbastecidos = bomba.LitrosAbastecidos;
            existente.BicoNoColdre = bomba.BicoNoColdre;
            existente.ModoOperacao = bomba.ModoOperacao;
            existente.LimiteValorPredeterminado = bomba.LimiteValorPredeterminado;
        }
    }

    public IReadOnlyList<Produto> ObterProdutos() => _produtos.AsReadOnly();

    public Produto? ObterProdutoPorId(int idProduto) => _produtos.FirstOrDefault(p => p.IdProduto == idProduto);

    public void SalvarProduto(Produto produto)
    {
        if (produto.IdProduto == 0)
        {
            produto.IdProduto = _produtos.Count > 0 ? _produtos.Max(p => p.IdProduto) + 1 : 1;
            _produtos.Add(produto);
        }
        else
        {
            var existente = ObterProdutoPorId(produto.IdProduto);
            if (existente != null)
            {
                existente.Nome = produto.Nome;
                existente.Preco = produto.Preco;
                existente.QuantidadeEstoque = produto.QuantidadeEstoque;
                existente.EstoqueMinimo = produto.EstoqueMinimo;
                existente.Categoria = produto.Categoria;
            }
        }
    }

    // RN19: Ajuste manual com aprovação de gerente
    public void RealizarAjusteEstoque(int idProduto, int novaQuantidade, string motivo, Gerente gerente)
    {
        var produto = ObterProdutoPorId(idProduto);
        if (produto != null && gerente.PodeAutorizarAjusteEstoque())
        {
            produto.QuantidadeEstoque = novaQuantidade;
        }
    }

    public IReadOnlyList<Cliente> ObterClientes() => _clientes.AsReadOnly();

    public Cliente? ObterClientePorDoc(string cpfCnpj)
    {
        string limpo = new string(cpfCnpj.Where(char.IsDigit).ToArray());
        return _clientes.FirstOrDefault(c => new string(c.CpfCnpj.Where(char.IsDigit).ToArray()) == limpo);
    }

    public void SalvarCliente(Cliente cliente)
    {
        if (cliente.IdCliente == 0)
        {
            cliente.IdCliente = _clientes.Count > 0 ? _clientes.Max(c => c.IdCliente) + 1 : 1;
            _clientes.Add(cliente);
        }
        else
        {
            var existente = _clientes.FirstOrDefault(c => c.IdCliente == cliente.IdCliente);
            if (existente != null)
            {
                existente.Nome = cliente.Nome;
                existente.Telefone = cliente.Telefone;
                existente.Email = cliente.Email;
                existente.Endereco = cliente.Endereco;
                existente.DividaPendente = cliente.DividaPendente;
                existente.DataVencimentoDivida = cliente.DataVencimentoDivida;
            }
        }
    }

    // RN09: Cliente fugiu sem pagar
    public void RegistrarFugaNaoPaga(string placaOuCpf, decimal valor, DateTime dataAtual)
    {
        DateTime vencimento = new DateTime(dataAtual.Year, dataAtual.Month, 1).AddMonths(1).AddDays(4); // Dia 5 do mês seguinte

        var cliente = ObterClientePorDoc(placaOuCpf) ?? _clientes.FirstOrDefault(c => c.PlacaVeiculoRegistroDivida == placaOuCpf);
        if (cliente != null)
        {
            cliente.DividaPendente += valor;
            cliente.DataVencimentoDivida = vencimento;
        }
        else
        {
            _clientes.Add(new Cliente
            {
                IdCliente = _clientes.Max(c => c.IdCliente) + 1,
                Nome = $"Veículo Placa {placaOuCpf.ToUpper()}",
                CpfCnpj = placaOuCpf,
                PlacaVeiculoRegistroDivida = placaOuCpf.ToUpper(),
                DividaPendente = valor,
                DataVencimentoDivida = vencimento,
                DataCadastro = dataAtual
            });
        }
    }

    public IReadOnlyList<Venda> ObterVendas() => _vendas.AsReadOnly();

    public Venda RegistrarVenda(Venda venda)
    {
        venda.IdVenda = _vendas.Count > 0 ? _vendas.Max(v => v.IdVenda) + 1 : 1;
        venda.DataHora = DateTime.Now;

        // Deduz estoque dos produtos
        foreach (var item in venda.ItensProdutos)
        {
            var prod = ObterProdutoPorId(item.Produto.IdProduto);
            if (prod != null)
            {
                prod.QuantidadeEstoque = Math.Max(0, prod.QuantidadeEstoque - item.Quantidade);
            }
        }

        // Atualiza caixa
        if (venda.Pagamento != null)
        {
            switch (venda.Pagamento.Forma)
            {
                case FormaPagamento.Dinheiro: _sessaoAtual.TotalDinheiro += venda.Pagamento.Valor; break;
                case FormaPagamento.CartaoDebito: _sessaoAtual.TotalCartaoDebito += venda.Pagamento.Valor; break;
                case FormaPagamento.CartaoCredito: _sessaoAtual.TotalCartaoCredito += venda.Pagamento.Valor; break;
                case FormaPagamento.Cheque: _sessaoAtual.TotalCheque += venda.Pagamento.Valor; break;
                case FormaPagamento.APrazo:
                    _sessaoAtual.TotalAPrazo += venda.Pagamento.Valor;
                    if (venda.Cliente != null)
                    {
                        venda.Cliente.DividaPendente += venda.Pagamento.Valor;
                    }
                    break;
            }
        }

        // RN03: Gastou R$ 200,00 ou mais -> Ganha vale-lavagem de graça com 30 dias de validade
        if (venda.ValorTotal >= 200.00m && venda.Cliente != null)
        {
            var voucher = new BonusLavagem
            {
                IdBonus = _vouchers.Count > 0 ? _vouchers.Max(v => v.IdBonus) + 1 : 1,
                CodigoVoucher = $"LAV-{new Random().Next(1000, 9999)}",
                Cliente = venda.Cliente,
                ValorGastoOrigem = venda.ValorTotal,
                DataEmissao = DateTime.Now,
                Utilizado = false
            };
            _vouchers.Add(voucher);
            venda.BonusLavagemGerado = voucher;
        }

        // Acúmulo de gastos
        if (venda.Cliente != null)
        {
            venda.Cliente.TotalGastoAcumulado += venda.ValorTotal;
        }

        _vendas.Add(venda);
        return venda;
    }

    // RN17: Cancelamento até 30 minutos
    public bool CancelarVenda(int idVenda, string motivo, DateTime agora)
    {
        var venda = _vendas.FirstOrDefault(v => v.IdVenda == idVenda);
        if (venda == null || !venda.PodeCancelar(agora)) return false;

        venda.Status = StatusVenda.Cancelada;
        venda.DataCancelamento = agora;
        venda.MotivoCancelamento = motivo;

        // Devolve estoque
        foreach (var item in venda.ItensProdutos)
        {
            var prod = ObterProdutoPorId(item.Produto.IdProduto);
            if (prod != null) prod.QuantidadeEstoque += item.Quantidade;
        }

        return true;
    }

    public NotaFiscal EmitirNotaFiscal(int idVenda)
    {
        var venda = _vendas.FirstOrDefault(v => v.IdVenda == idVenda) ?? throw new InvalidOperationException("Venda não encontrada.");
        var nf = new NotaFiscal
        {
            NumeroNF = 1000 + venda.IdVenda,
            DataEmissao = DateTime.Now,
            IdVendaOrigem = venda.IdVenda,
            ValorTotal = venda.ValorTotal,
            CpfCnpjDestinatario = venda.Cliente?.CpfCnpj ?? "Consumidor Final",
            NomeDestinatario = venda.Cliente?.Nome ?? "Consumidor Final Não Identificado"
        };
        venda.NotaFiscalEmitida = nf;
        return nf;
    }

    public IReadOnlyList<BonusLavagem> ObterVouchers() => _vouchers.AsReadOnly();

    public BonusLavagem? ValidarVoucher(string codigoVoucher, DateTime dataAtual)
    {
        var v = _vouchers.FirstOrDefault(x => string.Equals(x.CodigoVoucher, codigoVoucher, StringComparison.OrdinalIgnoreCase));
        if (v != null && v.EstaValido(dataAtual))
        {
            return v;
        }
        return null;
    }

    public IReadOnlyList<Funcionario> ObterFuncionarios() => _funcionarios.AsReadOnly();

    public Funcionario? ObterFuncionarioPorUsuario(string usuario, string senha)
    {
        return _funcionarios.FirstOrDefault(f => f.Usuario == usuario && f.Senha == senha && f.Ativo);
    }

    public void SalvarFuncionario(Funcionario funcionario)
    {
        if (funcionario.IdFuncionario == 0)
        {
            funcionario.IdFuncionario = _funcionarios.Count > 0 ? _funcionarios.Max(f => f.IdFuncionario) + 1 : 1;
            _funcionarios.Add(funcionario);
        }
        else
        {
            var existente = _funcionarios.FirstOrDefault(f => f.IdFuncionario == funcionario.IdFuncionario);
            if (existente != null)
            {
                existente.Nome = funcionario.Nome;
                existente.Cargo = funcionario.Cargo;
                existente.Telefone = funcionario.Telefone;
                existente.Endereco = funcionario.Endereco;
                existente.Salario = funcionario.Salario;
                existente.Ativo = funcionario.Ativo;
            }
        }
    }

    public void DesativarFuncionario(int idFuncionario)
    {
        var f = _funcionarios.FirstOrDefault(x => x.IdFuncionario == idFuncionario);
        if (f != null) f.Ativo = false;
    }

    public SessaoCaixa ObterSessaoCaixaAtual() => _sessaoAtual;

    // RN16: Fechamento validado por Gerente
    public void FecharSessaoCaixa(Gerente gerenteValidador)
    {
        _sessaoAtual.DataFechamento = DateTime.Now;
        _sessaoAtual.GerenteValidador = gerenteValidador;
    }

    public IReadOnlyList<Fornecedor> ObterFornecedores() => _fornecedores.AsReadOnly();

    public void SalvarFornecedor(Fornecedor fornecedor)
    {
        if (fornecedor.IdFornecedor == 0)
        {
            fornecedor.IdFornecedor = _fornecedores.Count > 0 ? _fornecedores.Max(f => f.IdFornecedor) + 1 : 1;
            _fornecedores.Add(fornecedor);
        }
        else
        {
            var existente = _fornecedores.FirstOrDefault(f => f.IdFornecedor == fornecedor.IdFornecedor);
            if (existente != null)
            {
                existente.RazaoSocial = fornecedor.RazaoSocial;
                existente.Cnpj = fornecedor.Cnpj;
                existente.Telefone = fornecedor.Telefone;
                existente.Email = fornecedor.Email;
            }
        }
    }

    public void QuitarDividaCliente(int idCliente)
    {
        var cli = _clientes.FirstOrDefault(c => c.IdCliente == idCliente);
        if (cli != null)
        {
            cli.DividaPendente = 0;
            cli.DataVencimentoDivida = null;
            cli.PlacaVeiculoRegistroDivida = null;
        }
    }

    public IReadOnlyList<CompraMercadoria> ObterComprasMercadorias() => _comprasMercadorias.AsReadOnly();

    public void RegistrarCompraMercadoria(CompraMercadoria compra)
    {
        compra.IdCompra = _comprasMercadorias.Count > 0 ? _comprasMercadorias.Max(c => c.IdCompra) + 1 : 1;
        compra.Data = DateTime.Now;
        _comprasMercadorias.Add(compra);

        // Se for conveniência, incrementa o estoque do produto se existir
        if (compra.Categoria == "Conveniência")
        {
            var prod = _produtos.FirstOrDefault(p => string.Equals(p.Nome, compra.NomeItem, StringComparison.OrdinalIgnoreCase));
            if (prod != null)
            {
                prod.QuantidadeEstoque += (int)compra.Quantidade;
            }
        }
    }

    public IReadOnlyList<RelatorioHorasFuncionario> GerarRelatorioHorasTrabalhadas()
    {
        var lista = new List<RelatorioHorasFuncionario>();
        foreach (var f in _funcionarios)
        {
            int marcacoes = _pontos.Count(p => p.Funcionario.IdFuncionario == f.IdFuncionario);
            // Simulação de cálculo de jornada mensal
            double horasNormais = 176.0;
            double horasExtras = marcacoes > 2 ? 8.5 : 0.0;
            int dias = 22;

            lista.Add(new RelatorioHorasFuncionario
            {
                NomeFuncionario = f.Nome,
                Cargo = f.Cargo,
                Matricula = f.Matricula,
                HorasTrabalhadasMes = horasNormais + horasExtras,
                HorasExtras = horasExtras,
                DiasTrabalhados = dias,
                StatusJornada = f.Ativo ? "Regular (Jornada 100% Cumprida)" : "Desligado / Inativo"
            });
        }
        return lista.AsReadOnly();
    }

    public IReadOnlyList<RegistroPonto> ObterRegistrosPonto() => _pontos.AsReadOnly();

    public void RegistrarPonto(Funcionario funcionario, TipoMarcacaoPonto tipo)
    {
        _pontos.Add(new RegistroPonto
        {
            IdRegistro = _pontos.Count + 1,
            Funcionario = funcionario,
            Tipo = tipo,
            DataHora = DateTime.Now
        });
    }
}
