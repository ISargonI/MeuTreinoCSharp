# Auto Posto Oásis - Documento de Estudo 09: Auditoria de Pista e Caixa, Alertas de Combustível (RN12/RN13) e Gestão Visual de RH

Este documento consolida as implementações realizadas para atender aos refinamentos solicitados nos perfis de **Gerente** e **Auxiliar Administrativo / RH**, alinhando-se aos requisitos da disciplina de POO e Interfaces de Usuário (Prof. Dr. Alan Gavioli - UTFPR).

---

## 1. Problema 1: Auditoria Analítica de Pista no Perfil do Gerente

### Diagnóstico
Na tela do Gerente, ao abrir os detalhes de faturamento da pista (*drill-down*), os campos de funcionário responsável e combustível ficavam em branco devido a bindings desatualizados (`FuncionarioOperador` em vez de `Operador` / `Frentista` e `AbastecimentoPista` em vez de `ItemCombustivel`). Além disso, não havia diferenciação explícita entre quem realizou o abastecimento físico na pista (**Frentista**) e quem recebeu o pagamento no caixa (**Operador de Caixa**).

### Solução Implementada
1. **Modelagem de Domínio (`Venda.cs`)**:
   * Adicionada a propriedade `Frentista` na classe `Venda`.
   * Adicionadas propriedades auxiliares formatadas:
     * `NomeFrentista`: Nome do frentista que operou a bomba na pista (ex: `Wilson Silva (Frentista)`).
     * `NomeOperadorCaixa`: Nome do operador de caixa que fechou a venda (ex: `Travis Bickle (Caixa)`).
     * `NomeCliente`: Identificação do cliente cadastrado ou consumidor final.
     * `DescricaoCombustivel`: Mostra a Bomba, o Tipo de Combustível, Litros abastecidos e Preço por Litro.
     * `FormaPagamentoTexto`: Dinheiro, Cartão Débito, Cartão Crédito, Cheque ou A Prazo.
2. **Dados Iniciais de Auditoria (`PostoRepository.cs`)**:
   * Foram inseridas vendas iniciais simuladas de pista e de conveniência no repositório com dados completos (horários, frentistas, caixas, clientes e litros) para que o Gerente já disponha de histórico real para auditoria imediata e captura de prints acadêmicos.

---

## 2. Problema 2: Alertas de Tanques de Combustível em Nível Crítico (RN12 / RN13)

### Diagnóstico
O card de estoque baixo da gerência considerava apenas os produtos de conveniência (bebidas e lanches), sem alertar sobre o nível crítico dos tanques de combustível subterrâneos.

### Regras do Trabalho Atendidas
* **RN13 (Alerta de Reabastecimento)**: O gerente deve ser alertado quando o combustível do tanque atingir **<= 15%** da capacidade total, indicando a necessidade de compra imediata da Petrobras (**RN15**).
* **RN12 (Corte de Segurança)**: O sistema bloqueia as bombas quando o tanque atinge **<= 4%** para não sugar sujeira ou queimar motores.

### Solução Implementada
1. **Banner Prominente de Alerta Gerencial (`GerenteView.axaml`)**:
   * Adicionado um alerta em destaque no topo do painel da Gerência listando os tanques em estado crítico:
     * 🚨 **Tanque de Etanol**: 3.8% (760 L / 20.000 L) - **[CORTE CRÍTICO < 4% - BOMBAS BLOQUEADAS RN12]**.
     * ⚠️ **Tanque de Diesel**: 12.0% (2.400 L / 20.000 L) - **[ALERTA DE REPOSIÇÃO <= 15% RN13]**.
     * Indicação clara do fornecedor homologado exclusivo (**Petrobras S.A. - RN15**).
2. **Card Executivo Unificado**:
   * O card 4 exibe a soma de alertas: `2 Tanques + 2 Itens`.
   * Ao clicar no card, o painel analítico exibe separadamente:
     * **1. Tanques de Combustível em Alerta [RN12/RN13]**;
     * **2. Produtos da Conveniência Abaixo do Estoque Mínimo [RN14]**.

---

## 3. Problema 3: Visualização Imediata de Novos Colaboradores no RH

### Diagnóstico
Ao cadastrar um novo colaborador, os dados eram persistidos no repositório e a coleção era recarregada, porém a tabela de colaboradores cadastrados ficava posicionada no final da página (abaixo de fornecedores e compras), forçando uma rolagem longa e dando a impressão de que o funcionário não havia sido listado.

### Solução Implementada
* **Reorganização do Layout em 2 Colunas Paralelas (`AuxiliarAdmView.axaml`)**:
  * **Coluna Esquerda (Gestão de RH & Pessoal)**:
    1. Formulário de Admissão de Colaborador (**RF15 / RN04 / RN05**);
    2. Relação de Colaboradores Cadastrados (**RF18**), posicionada **diretamente abaixo do formulário**, permitindo visualização instantânea do novo funcionário sem qualquer rolagem.
  * **Coluna Direita (Suprimentos & Fornecedores)**:
    1. Cadastro de Fornecedor da Conveniência (**RF13 / RN08**);
    2. Recebimento de Caminhão-Tanque Petrobras (**RF09 / RN15**);
    3. Relação de Fornecedores Homologados (**RN15 / RN08**).
* **Validação de Salário com Fallback Amigável**: Caso o campo de salário não seja preenchido ou informado com valor zero, o sistema preenche automaticamente o piso nacional da categoria selecionada (**RN05**).
