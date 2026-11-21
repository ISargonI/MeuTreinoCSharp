# Auto Posto Oásis - Guia de Arquitetura em Camadas e Projeto Avalonia UI

> **Disciplina:** Programação Orientada a Objetos e Modelagem de Software  
> **Instituição:** Universidade Tecnológica Federal do Paraná (UTFPR) - Câmpus Medianeira  
> **Docente:** Prof. Dr. Alan Gavioli  
> **Estudante:** Adriano Rodrigues Alves  

---

## 1. Introdução: Entendendo o que é uma Solução e seus Projetos em .NET

No ecossistema .NET e C#, quando desenvolvemos um software profissional de médio ou grande porte, não colocamos todo o código em um único arquivo ou em uma única pasta. Nós organizamos o sistema em uma **Solução (.sln)** composta por múltiplos **Projetos (.csproj)**.

### A analogia do mundo real
* **A Solução (`.sln`)**: Imagine uma pasta fichário. Ela não contém código executável por si só; sua função é apenas listar e manter juntos todos os projetos que fazem parte daquele sistema.
* **Os Projetos (`.csproj`)**: São os cadernos ou seções dentro do fichário. Cada projeto tem um propósito específico, compila de forma independente e gera:
  * Uma **Biblioteca de Classes (`.dll`)**: Código que contém regras, cálculos ou acesso a dados, sem interface gráfica direta.
  * Um **Aplicativo Executável (`.exe`)**: O programa que o usuário inicia com dois cliques (no nosso caso, a interface gráfica Avalonia UI).

---

## 2. A Arquitetura em Camadas (Por que separar?)

Separar a aplicação em camadas respeita princípios fundamentais de engenharia de software e POO, tais como:
* **Princípio da Responsabilidade Única (SRP)**: Cada camada cuida exclusivamente da sua especialidade.
* **Baixo Acoplamento**: Se amanhã decidirmos trocar o Avalonia UI por uma interface Web ou aplicativo móvel, a camada de negócio e as regras do posto não sofrem nenhuma alteração.
* **Facilidade de Teste e Manutenção**: Erros de cálculo financeiro ou de validação de regras de bombas são corrigidos no Domínio, sem correr o risco de quebrar o layout da tela.

### Divisão proposta para o Auto Posto Oásis:

```
AutoPostoOasis/
├── AutoPostoOasis.sln               # Solução principal
├── docs/                            # Documentação, estudos e guias
├── assets/                          # Logos, ícones e recursos visuais
└── src/
    ├── PostoOasis.Core/             # CAMADA DE DOMÍNIO E REGRAS DE NEGÓCIO (Class Library)
    │   ├── Models/                  # Entidades: Cliente, Funcionario, Bomba, Tanque, Venda...
    │   ├── BusinessRules/           # Validadores: CPF, CNPJ, Estoque, Nível de Tanque...
    │   └── Interfaces/              # Contratos: IClienteRepository, IVendaRepository...
    │
    ├── PostoOasis.Data/             # CAMADA DE PERSISTÊNCIA (Class Library)
    │   ├── Context/                 # Conexão e controle de arquivos / dados
    │   └── Repositories/            # Implementação real de IClienteRepository, etc.
    │
    └── PostoOasis.App/              # CAMADA DE APRESENTAÇÃO (Avalonia UI MVVM)
        ├── Views/                   # Telas em XAML (MainWindow, AbastecimentoView, etc.)
        ├── ViewModels/              # Conexão reativa entre a View e o Core
        └── Assets/                  # Imagens, logo e ícones do Posto Oásis
```

### O que vai em cada camada?

1. **Camada de Domínio / Negócio (`PostoOasis.Core`):**
   * É o coração do sistema. Não depende de nenhuma biblioteca gráfica (nada de Avalonia aqui).
   * Contém as classes modeladas no diagrama de classes do trabalho: `Funcionario`, `Gerente`, `Combustivel`, `Bomba`, `ItemCombustivel`, `Venda`, `ItemVenda`, `Produto`, `Cliente`, `BonusLavagem`, `NotaFiscal`, `Fornecedor`.
   * Contém os conceitos de POO trabalhados na disciplina:
     * **Encapsulamento:** propriedades privadas com getters/setters e métodos de controle.
     * **Equals e GetHashCode:** comparação correta de objetos (ex: dois clientes com mesmo CPF/CNPJ).
     * **Herança e Polimorfismo:** `Gerente` herdando de `Funcionario`.
     * **Associações (Composição e Agregação):** `Venda` composta por `ItemVenda` e `ItemCombustivel`; agregação entre `Cliente` e `Venda`.
   * Implementa todas as 20 Regras de Negócio (RN01 a RN20).

2. **Camada de Persistência (`PostoOasis.Data`):**
   * Responsável por salvar e recuperar as informações (em arquivos texto/JSON ou banco de dados relacional).
   * Conhece a camada `PostoOasis.Core` para saber quais objetos está salvando.
   * Não conhece a tela.

3. **Camada de Apresentação (`PostoOasis.App` com Avalonia UI):**
   * Responsável pela interação com o usuário (frentista, caixa, gerente, auxiliar administrativo).
   * Utiliza o padrão **MVVM (Model-View-ViewModel)** nativo do Avalonia:
     * **View:** Código XAML (`.axaml`) que desenha os botões, visores, gráficos e tabelas.
     * **ViewModel:** Classes C# que guardam os valores digitados na tela e acionam as regras da camada `Core`.
   * Segue as **Regras de Ouro de Interface** dos slides do Prof. Alan Gavioli.

---

## 3. Guia Prático: Como Criar os Projetos no VS Code Passo a Passo

Você executará estes comandos no terminal integrado do VS Code (PowerShell no Windows ou Bash no Linux/macOS).

### Passo 1: Instalar os templates oficiais do Avalonia UI
No terminal, execute o comando para registrar os modelos de projeto do Avalonia no seu .NET SDK:
```bash
dotnet new install Avalonia.Templates
```

### Passo 2: Criar a Solução (.sln)
Na pasta raiz do projeto (`ProjetoModelagemPosto`), crie o arquivo de solução:
```bash
dotnet new sln -n AutoPostoOasis
```

### Passo 3: Criar os Projetos de cada Camada

1. **Camada de Domínio/Negócio (Biblioteca de Classes):**
   ```bash
   dotnet new classlib -o src/PostoOasis.Core
   ```
2. **Camada de Persistência (Biblioteca de Classes):**
   ```bash
   dotnet new classlib -o src/PostoOasis.Data
   ```
3. **Camada de Apresentação (Avalonia UI com padrão MVVM):**
   ```bash
   dotnet new avalonia.mvvm -o src/PostoOasis.App
   ```

### Passo 4: Adicionar os Projetos à Solução
Vincule os três projetos criados ao arquivo da solução:
```bash
dotnet sln add src/PostoOasis.Core/PostoOasis.Core.csproj
dotnet sln add src/PostoOasis.Data/PostoOasis.Data.csproj
dotnet sln add src/PostoOasis.App/PostoOasis.App.csproj
```

### Passo 5: Estabelecer as Referências entre as Camadas (Dependências)
As camadas só podem enxergar aquilo que lhes diz respeito:
* A camada de **Persistência** precisa enxergar o **Domínio**:
  ```bash
  dotnet add src/PostoOasis.Data/PostoOasis.Data.csproj reference src/PostoOasis.Core/PostoOasis.Core.csproj
  ```
* A camada de **Apresentação** precisa enxergar o **Domínio** (para usar as classes e regras) e a **Persistência** (para gravar/ler os dados):
  ```bash
  dotnet add src/PostoOasis.App/PostoOasis.App.csproj reference src/PostoOasis.Core/PostoOasis.Core.csproj
  dotnet add src/PostoOasis.App/PostoOasis.App.csproj reference src/PostoOasis.Data/PostoOasis.Data.csproj
  ```

### Passo 6: Compilar e Testar a Solução
Para garantir que todos os projetos foram criados e conectados sem erros:
```bash
dotnet build
```
E para rodar a janela inicial do Avalonia UI:
```bash
dotnet run --project src/PostoOasis.App/PostoOasis.App.csproj
```

---

## 4. Projeto de Telas: Baseando-se nos Slides do Prof. Dr. Alan Gavioli

Os slides da disciplina estabelecem as **3 Regras de Ouro** (Theo Mandel / Sommerville / Pressman) e a distinção entre Wireframe, Mockup e Protótipo:

### 1. Deixar o usuário no comando
* **Interação flexível:** atalhos pelo teclado (ex: `F1` para abastecimento rápido, `F2` para caixa, `ESC` para fechar modais) além do mouse/toque.
* **Permitir interromper e desfazer:** botões claros de "Cancelar" e confirmação antes de ações críticas.
* **Ocultar detalhes técnicos:** mensagens amigáveis de erro (ex: "CPF inválido", em vez de "FormatException at line 42").

### 2. Reduzir a carga de memória do usuário
* **Metáforas do mundo real:** 
  * As bombas de combustível exibem visores digitais simulando as bombas reais da pista (litros, preço do litro, valor total).
  * Os tanques exibem medidores visuais verticais de nível com indicação de cores (verde: normal, amarelo: alerta < 15%, vermelho: crítico < 4%).
* **Defaults significativos:** forma de pagamento padrão selecionada (Dinheiro/PIX), data de hoje pré-preenchida.
* **Revelação progressiva (Progressive Disclosure):** telas de pagamento exibem primeiro a seleção da forma (Dinheiro, Cartão, Cheque, A Prazo) e só mostram os campos adicionais (ex: dados do cheque ou busca de cliente cadastrado) quando a opção correspondente for marcada.

### 3. Tornar a interface consistente
* **Identidade Visual Unificada:** Aplicação das cores da marca Auto Posto Oásis (Verde floresta/energia `#1B5E20`, Laranja/Dourado do Sol `#E65100` e `#FFA000`, fundo limpo neutro `#F8F9FA` ou `#1E1E1E` no tema escuro).
* **Navegação Lateral Fixa:** Menu lateral intuitivo utilizando os ícones fornecidos:
  1. ⛽ **Abastecimento & Pista**
  2. ⭐ **Fidelidade & Lavagem**
  3. 🛒 **Conveniência & Estoque**
  4. 🗺️ **Tanques & Infraestrutura / Mapa**
  5. 👥 **Clientes & Contas a Prazo**
  6. 💼 **Gestão / RH / Fechamento de Caixa**

---

## 5. Mapeamento Completo de Requisitos e Regras de Negócio (RN01 a RN20)

Para atender à exigência do professor:
> *"Todas as Regras referente ao levantamento de requisitos devem aparecer, e quando todas as telas estiverem prontas devemos tirar print de que Regra ou regras a tela está representando/dando acesso."*

Abaixo está o mapeamento exato de cada regra para as telas correspondentes:

| Regra | Descrição da Regra de Negócio | Tela de Apresentação (Avalonia UI) | Elemento Visual / Ação na Tela |
| :--- | :--- | :--- | :--- |
| **RN01** | Permitir pagamento em dinheiro, cheque, débito ou crédito | `CaixaView` / `PagamentoModal` | Opções de botões/rádios para cada modalidade de pagamento. |
| **RN02** | Permitir compra a prazo para clientes cadastrados | `PagamentoModal` (Opção "A Prazo") | Campo de busca de cliente; bloqueia a opção se o cliente não for selecionado. |
| **RN03** | Gerar vale-lavagem a cada R$ 200 gastos (validade de 30 dias) | `CaixaView` / `FidelidadeView` | Banner/Alerta automático após finalizar venda >= R$ 200 gerando o voucher com contagem regressiva de validade. |
| **RN04** | Nenhum funcionário cadastrado sem CPF válido | `FuncionarioCadastroView` | Validação matemática em tempo real do campo CPF com feedback visual (verde/vermelho). |
| **RN05** | Salário não inferior ao piso nacional da função | `FuncionarioCadastroView` | Validador no campo de salário informando o piso salarial mínimo exigido. |
| **RN06** | Apenas perfil "RH" pode alterar salários | `FuncionarioCadastroView` / `PerfilView` | Campo de salário bloqueado/desabilitado caso o usuário logado não seja do RH. |
| **RN07** | Funcionário só solicita férias após 12 meses de trabalho | `FuncionarioGestaoView` (Aba Férias) | Botão "Solicitar Férias" habilitado somente se tempo de admissão >= 1 ano. |
| **RN08** | Validar CNPJ de fornecedores da conveniência | `FornecedorCadastroView` | Validação matemática do CNPJ no cadastro com mensagem imediata de inconsistência. |
| **RN09** | Cliente sem pagamento (calote) deve quitar até dia 5 | `RegistroCaloteModal` / `CobrancaView` | Botão rápido para frentista lançar placa/veículo fujão, calculando o vencimento para o dia 05 do mês seguinte. |
| **RN10** | Apenas clientes cadastrados podem comprar a prazo | `PagamentoModal` | Bloqueio de finalização "A Prazo" para clientes avulsos ou não cadastrados. |
| **RN11** | Empresas recebem cobrança mensal por boleto | `FaturamentoEmpresasView` | Botão "Gerar Boleto Mensal Consolidado" para clientes com CNPJ. |
| **RN12** | Tanque não pode ficar abaixo de 4% (bloqueia bomba) | `TanquesDashboardView` / `PistaBombasView` | Nível do tanque com barra vermelha e badge **BOMBA BLOQUEADA (NÍVEL CRÍTICO < 4%)**. |
| **RN13** | Alerta ao atingir 15% (nível do tanque para compra) | `TanquesDashboardView` | Notificação amarela de alerta com botão direto para "Solicitar Compra de Combustível". |
| **RN14** | Alerta quando estoque mínimo de produtos for atingido | `EstoqueConvenienciaView` | Grid de estoque destacando linhas em amarelo/vermelho e badge de "Comprar Estoque". |
| **RN15** | Apenas um único fornecedor de combustível (bandeirado) | `CombustivelCompraView` | Dados do fornecedor pré-fixados/bloqueados para edição, indicando fornecedor exclusivo da bandeira. |
| **RN16** | Fechamento de caixa validado por perfil Gerente | `FechamentoCaixaView` | Solicitação de credenciais/senha de Gerente para homologar o fechamento do turno/dia. |
| **RN17** | Cancelamento de venda permitido em até 30 minutos | `HistoricoVendasView` | Botão "Estornar/Cancelar" disponível apenas para vendas realizadas há menos de 30 minutos. |
| **RN18** | Pagamento em cheque só para clientes com > 3 meses de cadastro e adimplentes | `PagamentoModal` (Opção "Cheque") | Validação de elegibilidade automática exibindo motivo caso o cliente não atenda aos 3 meses. |
| **RN19** | Ajustes manuais de estoque exigem senha de Gerente | `AjusteEstoqueModal` | Dialog modal solicitando autenticação de Gerente para justificar perdas/avarias. |
| **RN20** | Clientes inadimplentes após dia 5 bloqueados para compras a prazo | `ClientesView` / `PagamentoModal` | Tag vermelha **INADIMPLENTE (BLOQUEADO)** no cadastro do cliente, impedindo seleção no caixa a prazo. |

---

## 6. Próximos Passos
1. Você executará os comandos de criação da solução e dos projetos.
2. Na sequência, criaremos a estrutura das classes de Domínio (`PostoOasis.Core`), aplicando os conceitos de POO ensinados em sala de aula (herança, encapsulamento, equals/hashcode, polimorfismo).
3. Desenvolveremos a janela principal e os menus no Avalonia UI, incorporando a paleta de cores e o logo do Posto Oásis.
