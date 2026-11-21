# Auto Posto Oásis - Arquitetura MVC, Telas Específicas por Perfil e CRUDs

> **Disciplina:** Programação Orientada a Objetos e Modelagem de Software  
> **Instituição:** Universidade Tecnológica Federal do Paraná (UTFPR) - Câmpus Medianeira  
> **Docente:** Prof. Dr. Alan Gavioli  
> **Estudante:** Adriano Rodrigues Alves  
> **Data:** Novembro de 2026  

---

## 1. O Padrão Arquitetural MVC (Model-View-Controller)

Atendendo ao conteúdo ministrado em sala de aula, a aplicação foi reestruturada para o padrão **MVC**, amplamente utilizado no desenvolvimento de software comercial:

```
┌────────────────────────────────────────────────────────────────────────┐
│                        ARQUITETURA MVC APLICADA                        │
└────────────────────────────────────────────────────────────────────────┘

    [ VIEW ] (Camada de Apresentação - Avalonia UI)
       │  ▲
       │  │ Exibe telas específicas para cada perfil e captura ações
       ▼  │
    [ CONTROLLER ] (Controladores de Fluxo e Ações)
       │  ▲
       │  │ Executa regras de negócio, validações e atualiza a interface
       ▼  │
    [ MODEL & REPOSITÓRIO ] (Camadas Core e Data)
          Entidades POO: Cliente, Funcionario, Gerente, Bomba, Combustivel...
```

### Como cada parte foi implementada:

1. **Model (`PostoOasis.Core/Models`):**
   * Contém as entidades ricas com conceitos de POO: encapsulamento, herança (`Gerente : Funcionario`), associações (`Venda` com `ItemVenda` e `ItemCombustivel`), sobrescrita de `Equals` e `GetHashCode` (por CPF/CNPJ).
   * Contém os validadores matemáticos de CPF e CNPJ com algoritmos reais de dígitos verificadores em `Services/ValidadorDocumento.cs`.

2. **Controller (`PostoOasis.App/Controllers`):**
   * **`LoginController`:** Autenticação de credenciais e direcionamento do usuário de acordo com o perfil de acesso (UC16, RF16, RNF08).
   * **`FrentistaController`:** Liberação de bombas, leitura de pulsos de 0,01L, bloqueio de segurança e anotação de calotes (UC1, RF01, RN09, RN12).
   * **`CaixaController`:** Frente de caixa unificada, validação de meios de pagamento, bloqueio de inadimplentes, geração de vale-lavagem, emissão de NF-e e estorno até 30 min (UC2, UC3, UC4, RF02, RF03, RN01, RN02, RN03, RN10, RN17, RN18, RN20).
   * **`AuxiliarAdmController`:** Admissão com validação de CPF e piso salarial, desativação de usuário, cadastro de fornecedores com CNPJ, recebimento de caminhão Petrobrás e compras de mercadorias (UC9, UC13, UC14, UC15, UC17, UC18, RF09, RF13, RF14, RF15, RF18, RN04, RN05, RN08, RN15).
   * **`GerenteController`:** CRUD de preços de combustível (RNF10), CRUD de produtos da conveniência (RF11), homologação de fechamento de caixa (RN16), homologação de quebras de estoque (RN19), emissão de boletos para empresas (RF05, RN11) e relatórios gerenciais (RF20).

3. **View (`PostoOasis.App/Views`):**
   * Telas específicas e independentes: `LoginView`, `FrentistaView`, `CaixaView`, `AuxiliarAdmView`, `GerenteView`.
   * A janela principal `MainWindow.axaml` atua como o container mestre, exibindo a barra superior de status e permitindo alternar de usuário com o botão **"Sair / Trocar Usuário ➔"**.

---

## 2. A Tela Inicial de Login (UC16 / RF16 / RNF08)

Conforme solicitado, o sistema inicia diretamente na **`LoginView`**:
* **Seleção Visual Rápida de Perfil:** Quatro cartões elegantes com ícones representativos:
  1. ⛽ **Frentista:** Pista, bombas, pulsos e fugas.
  2. 💳 **Caixa:** PDV, formas de pagamento, NF-e e vale-ducha.
  3. 📋 **Auxiliar Adm / RH:** Colaboradores, ponto eletrônico, compras e fornecedores.
  4. 👔 **Gerente:** CRUDs de preços e produtos, auditoria e relatórios.
* **Autenticação por Usuário e Senha (UC16):** Campos para digitar credenciais individuais com verificação de status ativo do usuário.

---

## 3. Telas Específicas por Tipo de Usuário e Requisitos Atendidos

### 1. Tela do Frentista (`FrentistaView`)
* **Mostradores Digitais das 6 Bombas (RF01):** Displays LCD de alta fidelidade visual (verde fluorescente) indicando litros bombeados e valor total da conta.
* **Leitura de Pulsos:** O botão `+1L (100 Pulsos)` simula a entrada de pulsos de hardware (cada pulso = 0,01 litro).
* **Dois Modos de Abastecimento:** Botão `Livre (Normal)` e botão `Pré-fixar R$ 50` (modo especial).
* **Bloqueio de Segurança Crítica (RN12):** As bombas de Etanol (#03 e #04) aparecem bloqueadas com a indicação `🚨 BLOQUEADA: TANQUE CRÍTICO (< 4%) [RN12]`.
* **Registro de Fuga sem Pagar / Calote (RN09):** O frentista digita a placa ou CPF do fujão e o sistema calcula a data de quitação automaticamente para o **dia 05 do mês seguinte**.
* **Ponto Eletrônico (RF17):** Botões para registro de Entrada, Saída Almoço, Retorno e Saída com data/hora precisa.

### 2. Tela do Caixa (`CaixaView`)
* **Integração Pista e Loja (RF01 + RF02):** Mostra os abastecimentos finalizados na pista e permite incluí-los com um clique no cupom fiscal juntamente com os produtos da conveniência.
* **Formas de Pagamento (RN01):** Dinheiro, Cartão Débito, Cartão Crédito, Cheque e A Prazo.
* **Validação de Venda a Prazo (RN02 / RN10 / RN20):** Só permite fiado para clientes cadastrados. Se o cliente selecionado tiver dívida vencida (inadimplente), o sistema bloqueia e emite aviso em destaque vermelho.
* **Validação de Cheque (RN18):** Bloqueia pagamento em cheque para clientes com cadastro recente (< 3 meses) ou que possuam dívidas pendentes.
* **Vale-Ducha / Lavagem Gratuita (RN03):** Se o total da venda for $\ge \text{R\$} 200,00$, o sistema emite automaticamente um voucher com código único e validade de 30 dias.
* **Nota Fiscal Eletrônica (RF03):** Botão "Emitir NF-e" gerando o registro fiscal com chave de acesso.
* **Cancelamento até 30 Minutos (RN17):** Botão "Cancelar [RN17]" que estorna a venda e devolve os itens ao estoque caso a venda tenha sido emitida há menos de 30 minutos.

### 3. Tela do Auxiliar Administrativo / RH (`AuxiliarAdmView`)
* **Admissão de Colaboradores (RF15):** Cadastro completo (nome, endereço, CTPS, telefone, cargo, salário).
* **Validação Matemática de CPF (RN04):** Impede o cadastro de CPFs inválidos.
* **Controle de Piso Salarial (RN05):** Rejeita salários abaixo do piso nacional da categoria.
* **Desativação de Usuário Desligado (RF18):** Botão de desativação imediata para garantir a segurança da informação.
* **Cadastro de Fornecedores da Conveniência (RF13):** Validação matemática de CNPJ (**RN08**).
* **Recebimento de Combustível (RF09 / RN15):** Descarga de caminhão-tanque associada ao fornecedor único e exclusivo da bandeira (*Petrobras Distribuidora S.A.*).

### 4. Tela do Gerente (`GerenteView`) - CRUDs e Controle Total
* **CRUD de Preços de Combustíveis (RNF10):**
  * Painel com caixas de edição para Gasolina, Etanol e Diesel.
  * O gerente altera o preço e clica em "Atualizar Preço"; o valor é sincronizado instantaneamente em todas as bombas da pista.
* **CRUD de Produtos da Loja de Conveniência (RF11 / RNF10):**
  * Tabela com todos os produtos, preços e estoques.
  * Formulário completo para cadastrar novos produtos ou editar produtos existentes (Nome, Preço de Venda, Categoria, Quantidade e Estoque Mínimo).
  * Botões "Salvar Produto" e "Editar no CRUD".
* **Homologação de Ajuste Manual de Estoque (RN19):**
  * Seleção de produto avariado ou vencido, campo de nova quantidade real e motivo da quebra/perda, homologado pelo Gerente.
* **Homologação do Fechamento de Caixa Diário (RF19 / RN16):**
  * Consolidação de todos os recebimentos do turno e botão de validação pelo Gerente.
* **Cobrança Mensal por Boleto para Empresas (RF05 / RN11):**
  * Botão "Gerar Boleto Mensal [RN11]" disponível para clientes PJ com débitos acumulados.
* **Relatórios Gerenciais de Vendas e Estoque (RF20):**
  * Indicadores de faturamento da pista, faturamento da conveniência, total de dívidas ativas e contagem de itens em estoque crítico (**RN14**).

---

## 4. Guia para Captura dos "Prints" para a Entrega Acadêmica

Cada tela foi construída contendo no seu topo e nos seus cartões **badges coloridos com os códigos dos Casos de Uso (UC), Requisitos Funcionais (RF), Requisitos Não-Funcionais (RNF) e Regras de Negócio (RN)**.

| Print | Tela a Acessar | O que demonstrar no Print |
| :---: | :--- | :--- |
| **01** | `LoginView` | Mostrar a tela inicial com os 4 cartões de perfil (`Frentista`, `Caixa`, `Auxiliar Adm`, `Gerente`) e os badges: **UC16, RF16, RNF08**. |
| **02** | `FrentistaView` | Mostrar as bombas com mostrador digital LCD em pulsos (**RF01**), as bombas de Etanol bloqueadas em vermelho (**RN12**) e o painel de fuga sem pagar (**RN09**). |
| **03** | `CaixaView` | Mostrar a montagem de venda com combustível e conveniência (**RF01/RF02**), as opções de pagamento (**RN01**), o banner do vale-ducha (**RN03**) e a lista com botões de NF-e (**RF03**) e cancelamento em 30 min (**RN17**). |
| **04** | `CaixaView` (Bloqueio) | Tentar finalizar uma venda "A Prazo" com o cliente inadimplente para capturar o print do bloqueio vermelho (**RN20**). |
| **05** | `AuxiliarAdmView` | Mostrar o formulário de admissão com validação de CPF (**RN04**) e piso salarial (**RN05**), a lista de colaboradores com botão de desativação (**RF18**) e o recebimento de combustível exclusivo Petrobrás (**RF09, RN15**). |
| **06** | `GerenteView` (Preços) | Mostrar o painel de **Reajuste Rápido de Preços de Combustível** (**RNF10**). |
| **07** | `GerenteView` (CRUD Produtos) | Mostrar a tabela de produtos e o formulário de **CRUD de Produtos** (**RF11, RNF10**). |
| **08** | `GerenteView` (Controles) | Mostrar os painéis de **Ajuste Manual de Estoque** (**RN19**), **Fechamento de Caixa** (**RN16**) e **Boleto Mensal de Empresas** (**RN11**). |
| **09** | `GerenteView` (Relatórios) | Mostrar os cards de indicadores executivos com faturamento e estoque crítico (**RF20**). |

---

## 5. Como Executar a Aplicação Agora

Abra o terminal e execute:

```powershell
dotnet run --project src/PostoOasis.App/PostoOasis.App.csproj
```

A aplicação abrirá diretamente na **Tela de Login**. Basta clicar em qualquer perfil para navegar na tela correspondente ou usar o botão "Sair / Trocar Usuário ➔" para retornar ao login a qualquer momento!
