# Auto Posto Oásis - Implementação das Camadas e Interface Avalonia UI (.NET 10)

> **Disciplina:** Programação Orientada a Objetos e Modelagem de Software  
> **Instituição:** Universidade Tecnológica Federal do Paraná (UTFPR) - Câmpus Medianeira  
> **Docente:** Prof. Dr. Alan Gavioli  
> **Estudante:** Adriano Rodrigues Alves  
> **Data:** Novembro de 2026  

---

## 1. Visão Geral da Solução Criada

A solução **Auto Posto Oásis** foi completamente estruturada e compilada com sucesso utilizando **.NET 10**, **Avalonia UI 11.2** (com suporte oficial consultado via Context7) e o padrão arquitetural **MVVM (Model-View-ViewModel)**.

```
AutoPostoOasis/
├── AutoPostoOasis.slnx                      # Solução .NET 10 moderna
├── docs/                                    # Documentação e guias de estudo
│   ├── 01-arquitetura-e-criacao-do-projeto.md
│   └── 02-implementacao-camadas-e-apresentacao-avalonia.md
├── assets/                                  # Logos e ícones originais
│   ├── logo-posto-oasis.png
│   └── icones-menu.jpg
└── src/
    ├── PostoOasis.Core/                     # CAMADA DE DOMÍNIO E REGRAS DE NEGÓCIO
    │   ├── Models/                          # Cliente, Funcionario, Gerente, Combustivel, Bomba, etc.
    │   ├── Interfaces/                      # IPostoRepository
    │   └── Services/                        # ValidadorDocumento (CPF e CNPJ matemáticos)
    │
    ├── PostoOasis.Data/                     # CAMADA DE PERSISTÊNCIA
    │   └── Repositories/                    # PostoRepository (com Seed realista de teste)
    │
    └── PostoOasis.App/                      # CAMADA DE APRESENTAÇÃO (Avalonia UI MVVM)
        ├── Assets/                          # Imagens e ícones embutidos
        ├── ViewModels/                      # MainWindowViewModel (Reatividade e regras)
        ├── Views/                           # MainWindow.axaml (Interface gráfica completa)
        ├── App.axaml                        # Estilos e tema escuro FluentTheme
        └── Program.cs                       # Ponto de entrada do aplicativo Desktop
```

---

## 2. Detalhamento dos Conceitos de POO Aplicados

No projeto `PostoOasis.Core`, foram aplicados rigorosamente todos os conceitos trabalhados na disciplina:

1. **Encapsulamento:**
   * Propriedades com validações internas e métodos com lógica de negócio encapsulada (ex: `Bomba.ReceberPulsoAbastecimento()`, `Combustivel.ConsumirLitros()`, `Cliente.PodePagarComCheque()`).
2. **Equals e GetHashCode:**
   * Sobrescritos em `Cliente`, `Funcionario`, `Fornecedor` e `Produto` para garantir comparações semânticas por chave primária/documento natural (ex: comparação de dois clientes pelo CPF/CNPJ limpo).
3. **Herança e Polimorfismo:**
   * A classe `Gerente` herda diretamente de `Funcionario` (`public class Gerente : Funcionario`), reaproveitando todos os atributos básicos e adicionando métodos e propriedades de autorização especial (alteração de salários, homologação de fechamento de caixa e autorização de perdas de estoque).
4. **Associações (Composição e Agregação):**
   * **Composição:** A classe `Venda` possui ciclo de vida vinculado a `ItemVenda` e `ItemCombustivel`. Se a venda deixa de existir, seus itens de detalhe também deixam.
   * **Agregação:** A `Venda` referencia um `Cliente` e um `Funcionario` (operador). O cliente e o funcionário existem independentemente de uma venda específica.
5. **Interfaces:**
   * `IPostoRepository` desacopla a camada de apresentação e negócio do mecanismo real de armazenamento de dados, permitindo trocar o repositório em memória por SQLite, PostgreSQL ou arquivos JSON sem alterar nenhuma regra de tela ou de negócio.

---

## 3. A Camada de Apresentação com Avalonia UI

A interface gráfica foi desenhada em conformidade estrita com os slides do **Prof. Dr. Alan Gavioli** sobre *Fundamentos de Interfaces do Usuário*:

### A. As Regras de Ouro Aplicadas:
* **Deixar o usuário no comando:**
  * O operador pode alternar livremente entre as telas através da barra lateral ou comandos.
  * Botões de cancelamento (ex: Limpar Carrinho, Estorno de Venda).
  * Mensagens claras de validação e feedback instantâneo sem expor stacktraces ou termos técnicos.
* **Reduzir a carga de memória:**
  * **Metáforas do mundo real:** Mostrador digital de bomba com dígitos verdes simulando displays LCD de bombas reais; medidores gráficos verticais/horizontais de tanques de combustível; crachás com contagem de validade dos vouchers de lavagem.
  * **Defaults inteligentes:** Pagamento em Dinheiro pré-selecionado, data atual pré-configurada, operador ativo exibido no rodapé da barra lateral.
* **Tornar a interface consistente:**
  * Identidade visual unificada com o logotipo original do **Auto Posto Oásis**, paleta de cores verde `#00E676` / `#00C853` e laranja solar `#FF6D00` / `#FFA000`, e tema escuro de alto contraste para ambientes de pista e conveniência.
  * Cada tela traz um cabeçalho padronizado contendo o título, subtítulo explicativo e **etiquetas/badges com os códigos de requisitos e regras de negócio correspondentes**.

---

## 4. Guia para Tirar os Prints Exigidos pelo Professor (RN01 a RN20)

O professor solicitou:
> *"Todas as Regras referente ao levantamento de requisitos devem aparecer, e quando todas as telas estiverem prontas devemos tirar print de que Regra ou regras a tela esta representando/dando acesso."*

Abaixo está o roteiro completo de onde cada regra aparece e como demonstrá-la para o print:

| Regra | Tela no Avalonia UI | Onde ver na tela / Como demonstrar para o print |
| :--- | :--- | :--- |
| **RF01** | `Pista & Bombas` | Mostrador LCD da bomba atualizando volume e total a cada pulso de 0,01L (botão "+1L (100 Pulsos)"). |
| **RN12** | `Pista & Bombas` / `Tanques` | As bombas de Etanol (#03 e #04) aparecem bloqueadas em vermelho com a mensagem: `🚨 BOMBA BLOQUEADA: TANQUE CRÍTICO (< 4%) [RN12]`. |
| **RN13** | `Tanques & Níveis` | O tanque de Gasolina (em 14%) exibe badge amarelo com o alerta: `⚠️ ALERTA DE COMPRA (< 15%) [RN13]`. |
| **RN15** | `Tanques & Níveis` | Card de recebimento de caminhão com fornecedor exclusivo fixo: `Petrobras S.A. [RN15]`. |
| **RN14** | `Loja Conveniência` | Produtos com estoque baixo (ex: *Óleo Lubrax 15W40* com 2 un) recebem badge vermelho: `BAIXO [RN14]`. |
| **RN19** | `Loja Conveniência` | Painel lateral "Ajuste de Estoque [RN19]" com campo obrigatório para a senha do Gerente. |
| **RN08** | `Loja Conveniência` | Formulário "Novo Fornecedor [RN08]" com validação matemática de 14 dígitos de CNPJ. |
| **RN01** | `Caixa & PDV` | Grupo de opções com as formas de pagamento: Dinheiro, Débito, Crédito, Cheque e A Prazo. |
| **RN02 / RN10** | `Caixa & PDV` | Selecionar "A Prazo" sem cliente selecionado emite o alerta: `[RN10 BLOQUEIO] Venda a prazo permitida apenas para clientes cadastrados!`. |
| **RN20** | `Caixa & PDV` / `Clientes` | Ao selecionar o cliente inadimplente (*Marcos Caloteiro*), o sistema emite o alerta vermelho: `[RN20 BLOQUEIO] Cliente está INADIMPLENTE (Dívida vencida dia 5)`. |
| **RN18** | `Caixa & PDV` | Ao tentar pagar em cheque com cliente com menos de 3 meses de cadastro, o sistema bloqueia exibindo a regra. |
| **RN03** | `Caixa & PDV` / `Clientes` | Ao finalizar uma venda de R$ 200,00 ou mais, é exibido o banner comemorativo com o voucher emitido: `⭐ [RN03 GANHOU LAVAGEM!] Voucher: LAV-XXXX (Validade de 30 dias)`. Na aba Clientes, os vouchers aparecem listados com a data de validade. |
| **RN09** | `Caixa & PDV` | Painel "Registro de Fuga sem Pagar [RN09]". Digitando uma placa e clicando em "Lançar Débito", o vencimento é calculado automaticamente para o **dia 05 do mês seguinte**. |
| **RF03** | `Caixa & PDV` | Botão "Emitir NF-e [RF03]" no histórico de vendas gerando a chave de acesso e comprovante fiscal. |
| **RN17** | `Caixa & PDV` | Botão "Cancelar [RN17]" no histórico de vendas, permitindo o cancelamento e estorno apenas se a venda tiver menos de 30 minutos. |
| **RN11** | `Clientes & Fidelidade` | Botão "Boleto Mensal [RN11]" visível para empresas (clientes com CNPJ), gerando a cobrança consolidada mensal. |
| **RN04** | `Gestão, RH & Turno` | Formulário de admissão rejeita colaboradores cujo CPF não passe na validação matemática de 11 dígitos. |
| **RN05** | `Gestão, RH & Turno` | Formulário rejeita admissão de funcionários com salário abaixo do piso nacional da função. |
| **RN06 / RN07** | `Gestão, RH & Turno` | Tabela de colaboradores exibe o crachá de elegibilidade para férias apenas para quem tem 12 meses ou mais de admissão. |
| **RF17** | `Gestão, RH & Turno` | Painel de controle de ponto eletrônico com botões de Entrada, Almoço e Saída com registro de data/hora. |
| **RN16** | `Gestão, RH & Turno` | Fechamento diário de caixa exigindo autenticação do Gerente para homologação dos valores apurados no turno. |

---

## 5. Como Executar a Aplicação

Para rodar a interface gráfica completa no seu computador:

```powershell
dotnet run --project src/PostoOasis.App/PostoOasis.App.csproj
```

A janela do **Auto Posto Oásis** abrirá em tela cheia/centralizada, pronta para navegação, demonstração de todas as regras e captura dos prints para o trabalho acadêmico.
