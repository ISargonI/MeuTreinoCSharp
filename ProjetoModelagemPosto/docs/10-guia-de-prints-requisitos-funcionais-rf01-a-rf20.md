# Guia Definitivo e Atualizado de Telas e Prints: 20 Requisitos Funcionais (RF01 a RF20)
**Projeto:** Auto Posto & Conveniência Oásis  
**Disciplina:** Programação Orientada a Objetos e Modelagem de Software (Prof. Dr. Alan Gavioli - UTFPR)  
**Tecnologia:** C# .NET 10, Avalonia UI 11.2 (Padrão MVC)  
**Base Oficial:** Levantamento de Requisitos Funcionais do Trabalho Prático

---

## 🧭 Como Executar e Navegar no Sistema

Para rodar a aplicação e tirar os prints:
```powershell
dotnet run --project src/PostoOasis.App/PostoOasis.App.csproj
```
*(Ou execute o arquivo compilado em `src\PostoOasis.App\bin\Debug\net10.0\PostoOasis.App.exe`)*.

- **Senha Padrão para todos os perfis:** `123`
- **Alternar Perfil:** Clique em **"⬅️ Sair / Trocar Perfil"** no topo superior esquerdo a qualquer momento.
- **Alternar Tema Claro/Escuro:** Botão **"🌓 Alternar Tema"** no canto superior direito.

---

## 📸 Roteiro Completo e Fiel: RF01 a RF20

---

### [RF01] Gerenciar bombas e abastecimento de combustível
* **Perfil:** `Frentista`
* **Descrição Oficial do PDF:** O sistema atua no gerenciamento integrado das bombas, com liberação prévia pelo Frentista, registro de pulso a cada fração de 1/100 de litro, sincronização em tempo real no mostrador digital (volume, preço/litro e total) e modos livre e predeterminado, gravando histórico de transações.
* **Onde encontrar na tela:** Painel central com as **6 Bombas de Combustível** com mostrador digital LCD verde fluorescente.
* **O que fazer para o print:**
  1. Na Bomba 01, clique em **"▶️ Iniciar Fluxo Livre"**.
  2. Veja os litros e valor subindo continuamente no display LCD.
  3. Clique em **"⏸️ Pausar Fluxo"** (o botão muda dinamicamente e os litros são preservados).
  4. Teste também o botão **"+1L Pulsos"** ou o modo pré-determinado de R$ 50,00.
* **O que destacar no print:** O display digital da bomba com Litros, Valor/Litro e Total a Pagar, os botões de controle de fluxo e o badge `[RF01] Gestão de Bombas`.

---

### [RF02] Gerenciar vendas da loja de conveniência
* **Perfil:** `Operador de Caixa`
* **Descrição Oficial do PDF:** O funcionário [Caixa] deve gerenciar e registrar as vendas dos produtos da loja de conveniência, integrando os valores às formas de pagamento do caixa.
* **Onde encontrar na tela:** Coluna esquerda (Catálogo de Produtos) e coluna central (Cupom Fiscal / Carrinho).
* **O que fazer para o print:**
  1. No catálogo de produtos da conveniência (Café Expresso, Salgado Assado, Refrigerante Lata, etc.), clique em **"Adicionar ao Carrinho"**.
  2. Observe a lista do carrinho detalhando quantidade, preço unitário e o subtotal acumulado em tempo real.
* **O que destacar no print:** Os produtos no carrinho com valor total calculado e o badge `[RF02] PDV Conveniência`.

---

### [RF03] Emitir nota fiscal
* **Perfil:** `Operador de Caixa`
* **Descrição Oficial do PDF:** O sistema deve emitir a nota fiscal referente aos pagamentos efetuados sempre que solicitada pelo cliente.
* **Onde encontrar na tela:** Tabela inferior "Histórico de Vendas Recentes".
* **O que fazer para o print:**
  1. Na linha de qualquer venda concluída, clique no botão azul **"📄 Emitir NF-e [RF03]"**.
  2. Observe a barra de mensagens exibindo a confirmação com chave de acesso de 44 dígitos e protocolo SEFAZ.
* **O que destacar no print:** O botão `📄 Emitir NF-e [RF03]` na tabela e a mensagem de confirmação da NF-e emitida.

---

### [RF04] Controlar pagamentos a prazo
* **Perfil:** `Operador de Caixa`
* **Descrição Oficial do PDF:** O sistema deve permitir e registrar compras faturadas a prazo exclusivamente para os clientes que possuírem seu cadastro completo na base de dados.
* **Onde encontrar na tela:** Painel direito "Finalizar Pagamento" e botão **"📑 A Prazo (Empresa) [RF04]"**.
* **O que fazer para o print (Demonstrando a Validação e Bloqueio):**
  1. Sem selecionar nenhum cliente cadastrado (ou selecionando "Consumidor Final"), clique em **"📑 A Prazo (Empresa) [RF04]"** e depois em **"FINALIZAR VENDA"**.
  2. O sistema emite o bloqueio: `⚠️ [RF04 BLOQUEIO] Venda a prazo permitida exclusivamente para clientes com cadastro completo na base de dados!`.
  3. Selecione o cliente **"Marcos Caloteiro Fujão"** ou a empresa inadimplente e tente vender a prazo: o sistema bloqueia imediatamente em vermelho com a mensagem: `⛔ [RF04 / RN20 BLOQUEIO TOTAL] Cliente está INADIMPLENTE com dívida vencida! Compras a prazo estão ESTRITAMENTE BLOQUEADAS.`.
* **O que destacar no print:** O botão de pagamento `[RF04]` e a mensagem em destaque com o bloqueio da transação para clientes não cadastrados ou inadimplentes.

---

### [RF05] Gerar boletos para empresas
* **Perfil:** `Gerente Geral`
* **Descrição Oficial do PDF:** O sistema deve somar os abastecimentos e consumos na conveniência do mês corrente e gerar um boleto bancário somente para as empresas cadastradas pagarem no final do mês.
* **Onde encontrar na tela:** Tabela "Clientes Corporativos Conveniados (Faturamento Faturado)" no final da tela do Gerente.
* **O que fazer para o print:**
  1. Localize a empresa cadastrada (ex: `Transportadora Rápido Medianeira Ltda - CNPJ: 12.345.678/0001-95`).
  2. Veja o total faturado no mês e o botão azul **"📑 Gerar Boleto Mensal [RN11 / RF05]"**.
  3. Clique no botão para simular a emissão do boleto bancário de cobrança com vencimento no dia 05.
* **O que destacar no print:** A listagem corporativa com o valor faturado do mês e o botão de geração de boleto bancário.

---

### [RF06] Gerenciar clientes
* **Perfil:** `Operador de Caixa` (ou `Frentista` para placa)
* **Descrição Oficial do PDF:** O sistema deve manter o gerenciamento de clientes, armazenando obrigatoriamente: CPF/CNPJ, nome completo, endereço, telefones de contato e e-mail. Permitir que o frentista anote a placa do veículo de clientes que saírem sem pagar criando registro de dívida até o dia 5. Permitir a busca e visualização dos dados cadastrais e histórico financeiro para autorização de vendas a prazo.
* **Onde encontrar na tela:**
  - **No Caixa:** Botão **"➕ Novo Cliente [RF06]"** e Card de Situação Cadastral.
  - **No Frentista:** Painel **"Anotar Placa de Fuga / Saída sem Pagar"**.
* **O que fazer para o print:**
  1. No Caixa, clique em **"➕ Novo Cliente [RF06]"**.
  2. Preencha o formulário completo com: Nome Completo, CPF/CNPJ, Telefone, Endereço e E-mail.
  3. Clique em **"💾 Salvar Cliente [RF06]"**. O cliente é cadastrado e selecionado automaticamente!
  4. Veja o card com a situação financeira e histórico de dívidas do cliente selecionado.
* **O que destacar no print:** O formulário de cadastro de cliente aberto com os campos obrigatórios preenchidos e o card de auditoria financeira do cliente.

---

### [RF07] Controlar acúmulo de gastos
* **Perfil:** `Operador de Caixa` ou `Gerente Geral`
* **Descrição Oficial do PDF:** O sistema deve monitorar e contabilizar continuamente o valor gasto pelos clientes, somando consumos de combustível e da loja de conveniência.
* **Onde encontrar na tela:** No card do cliente selecionado no Caixa e na tabela de clientes do Gerente.
* **O que fazer para o print:**
  1. No Caixa, selecione o cliente **"João Silva Sauro"** (ou o novo cliente cadastrado).
  2. Veja o indicador: `Total gasto acumulado: R$ 380,00 [RF07]`.
  3. Realize uma compra e observe o total acumulado somando o consumo de combustível e da loja continuamente.
* **O que destacar no print:** A linha com o `Total gasto acumulado [RF07]` exibida no perfil do cliente.

---

### [RF08] Monitorar nível dos tanques
* **Perfil:** `Gerente Geral` (ou `Auxiliar Administrativo`)
* **Descrição Oficial do PDF:** O sistema deve monitorar ativamente os níveis de armazenamento de gasolina, etanol e diesel dos três tanques do posto.
* **Onde encontrar na tela:** Card 4 no topo do Gerente (clique para abrir) e tabela de monitoramento de combustíveis.
* **O que fazer para o print:**
  1. No Gerente, clique no Card 4 **"Alertas de Reposição e Nível dos Tanques"**.
  2. Visualize a tabela e barras de monitoramento dos 3 tanques subterrâneos com Capacidade Total (ex: 20.000 L), Nível Atual em Litros e Percentual exato.
* **O que destacar no print:** O painel analítico com a capacidade e volumetria em tempo real dos 3 tanques de combustível.

---

### [RF09] Gerenciar compras de combustível (Único Fornecedor)
* **Perfil:** `Auxiliar Administrativo / RH`
* **Descrição Oficial do PDF:** O funcionário [Aux. Administrativo] deve registrar o recebimento de combustível via caminhões tanque para atualizar os níveis dos tanques operacionais e manter um cadastro de todas as compras de combustível (data, combustível, quantidade, preço por litro e preço total).
* **Onde encontrar na tela:** Seção "Receber Caminhão-Tanque Petrobras [RF09 / RN15]" (coluna direita da tela do Auxiliar Administrativo).
* **O que fazer para o print:**
  1. Selecione o tanque (ex: `Tanque de Gasolina Comum`), informe a litragem (ex: `5000`) e clique em **"Registrar Descarga de Combustível [RF09]"**.
  2. Veja a confirmação de entrada associada à Petrobras Distribuidora com litragem e atualização imediata do estoque do tanque.
* **O que destacar no print:** O formulário de recebimento de combustível com o badge `[RF09 / RN15]` e a distribuidora exclusiva Petrobras.

---

### [RF10] Emitir alerta de nível baixo de combustível
* **Perfil:** `Gerente Geral`
* **Descrição Oficial do PDF:** O sistema deve emitir um alerta ao gerente sinalizando a necessidade de compra de combustível se o tanque ficar abaixo de 15% da capacidade. Se o nível descer abaixo de 4%, deve desativar as bombas.
* **Onde encontrar na tela:** Banner de Segurança em destaque vermelho e amarelo no topo da tela do Gerente.
* **O que fazer para o print:**
  1. Na tela do Gerente, capture o **Banner Vermelho de Alerta Crítico**:
     `Tanque de Etanol Hidratado: Nível 3,3% < 4% [CORTE CRÍTICO - BOMBAS BLOQUEADAS RN12]`.
  2. Veja também o aviso amarelo do Diesel com nível de 13,3% `<= 15% [ALERTA REPOSIÇÃO RN13]`.
* **O que destacar no print:** O banner com os alertas de 15% e o corte de segurança de 4% com bloqueio operacional das bombas.

---

### [RF11] Gerenciar venda de produtos da loja de conveniência (Cadastro e Estoque)
* **Perfil:** `Gerente Geral`
* **Descrição Oficial do PDF:** O funcionário deve gerenciar o cadastro de itens da loja de conveniência, salvando os dados básicos, o preço atual, vendas e a quantidade em estoque, atualizando quantidades a cada venda.
* **Onde encontrar na tela:** Seção "Gestão de Produtos da Conveniência (CRUD)" no painel do Gerente.
* **O que fazer para o print:**
  1. Localize a tabela de produtos da conveniência e o formulário de cadastro/edição.
  2. Clique em **"Editar no CRUD"** em um item (ex: Café Expresso) ou preencha novo nome, categoria, preço, estoque e estoque mínimo.
  3. Clique em **"Cadastrar / Salvar Produto [RF11]"**.
* **O que destacar no print:** A tabela de produtos com o formulário de edição de preços e quantidades em estoque.

---

### [RF12] Emitir alerta de estoque mínimo [Produtos loja de conveniência]
* **Perfil:** `Gerente Geral`
* **Descrição Oficial do PDF:** O sistema deve notificar o gerente automaticamente sempre que a quantidade em estoque de um produto atingir o nível mínimo pré-configurado pelo próprio gerente.
* **Onde encontrar na tela:** Card 4 no topo ("ALERTAS DE ESTOQUE") e modal analítico de reposição.
* **O que fazer para o print:**
  1. Clique no Card 4 (`2 Tanques + 2 Itens`) no painel do Gerente.
  2. Veja a listagem detalhada de produtos que atingiram ou estão abaixo da cota mínima (ex: `Óleo Motor Lubrax 15W40: Estoque 2 un <= Mínimo 5 un` e `Barra de Chocolate: Estoque 3 un <= Mínimo 6 un`).
* **O que destacar no print:** O modal com a tabela de produtos em estado crítico de reposição comparando Estoque Atual e Estoque Mínimo.

---

### [RF13] Gerenciar fornecedores [Produtos loja de conveniência]
* **Perfil:** `Auxiliar Administrativo / RH`
* **Descrição Oficial do PDF:** O funcionário [Aux. Administrativo] deve manter os dados das empresas fornecedoras da conveniência, registrando CNPJ, nome completo, endereço, telefones e e-mail.
* **Onde encontrar na tela:** Seção "Cadastrar Fornecedor da Conveniência [RF13 / RN08]" e tabela "Fornecedores Homologados".
* **O que fazer para o print:**
  1. Preencha a Razão Social da distribuidora (ex: `Ambev Distribuidora`), CNPJ (14 dígitos válidos com validação anti-duplicidade), Telefone e E-mail.
  2. Veja logo abaixo a relação de fornecedores homologados para a conveniência.
* **O que destacar no print:** O formulário de cadastro de fornecedores com validação de CNPJ e a lista de parceiros comerciais cadastrados.

---

### [RF14] Gerenciar compras de produtos para loja de conveniência
* **Perfil:** `Auxiliar Administrativo / RH`
* **Descrição Oficial do PDF:** O funcionário [Aux. Administrativo] deve registrar a aquisição de mercadorias para a loja, armazenando data, fornecedor, nome do produto, quantidade, preço por unidade e preço total.
* **Onde encontrar na tela:** Seção **"Aquisição de Produtos para Conveniência [RF14]"** (coluna direita da tela do Auxiliar Administrativo).
* **O que fazer para o print:**
  1. Selecione o Produto (ex: `Café Expresso Especial`), o Fornecedor Homologado, informe a Quantidade (ex: `20`) e o Preço Unitário (ex: `4,50`).
  2. Clique em **"Registrar Aquisição de Mercadorias [RF14]"**.
  3. O sistema calcula o preço total, dá entrada no estoque do item e registra a compra.
* **O que destacar no print:** O formulário de entrada de compras com os campos de data, fornecedor, produto, quantidade e preço unitário.

---

### [RF15] Gerenciar funcionários
* **Perfil:** `Auxiliar Administrativo / RH`
* **Descrição Oficial do PDF:** O funcionário [Aux. Administrativo] deve gerenciar as informações dos colaboradores do posto ao armazenar código de identificação, nome, endereço, telefones de contato, cargo e número da carteira de trabalho de cada funcionário.
* **Onde encontrar na tela:** Formulário "Admissão de Colaboradores (RH) [RF15 / RN04 / RN05]" (coluna esquerda do Auxiliar Adm).
* **O que fazer para o print:**
  1. Veja os campos preenchidos: Nome Completo, CPF (11 dígitos), Cargo, Carteira de Trabalho (CTPS), Telefone, Endereço e Salário (validado com o piso nacional da categoria conforme RN05).
  2. Clique em **"Salvar Admissão de Colaborador [RF15]"** e veja o colaborador inserido na tabela abaixo.
* **O que destacar no print:** O formulário completo de admissão com o badge `[RF15]` e a tabela de funcionários ativos.

---

### [RF16] O funcionário deve realizar login com usuário e senha
* **Perfil:** `Tela Inicial de Login`
* **Descrição Oficial do PDF:** O acesso à plataforma é condicionado à autenticação do funcionário por meio do fornecimento de suas credenciais de usuário e senha.
* **Onde encontrar na tela:** Tela principal de autenticação do Posto Oásis.
* **O que fazer para o print:**
  1. Mostre a seleção de perfil (Frentista, Caixa, Auxiliar Adm ou Gerente).
  2. Mostre o campo de senha mascarado obrigatório (senha padrão `123`) e o botão **"Entrar no Sistema [RF16]"**.
  3. Teste clicar em Entrar com a senha em branco ou incorreta para capturar a mensagem de bloqueio de segurança.
* **O que destacar no print:** A tela de login institucional do Posto Oásis com a autenticação protegida por senha.

---

### [RF17] O sistema deve emitir relatório de horas trabalhadas por funcionário
* **Perfil:** `Auxiliar Administrativo / RH`
* **Descrição Oficial do PDF:** O sistema disponibilizará uma funcionalidade para a extração de relatórios de horas trabalhadas, permitindo o acompanhamento individual da jornada de cada funcionário pelo departamento de RH.
* **Onde encontrar na tela:** Painel **"Relatório de Horas Trabalhadas [RF17]"** (coluna direita da tela do Auxiliar Administrativo).
* **O que fazer para o print:**
  1. Localize a seção de horas e clique no botão azul **"📄 Extrair Relatório [RF17]"**.
  2. Observe a listagem com o espelho de ponto de cada funcionário (Wilson Castaway, Travis Bickle, Alan Gavioli, etc.), exibindo Horas Normais Trabalhadas no mês, Horas Extras, Dias Trabalhados e Status da Jornada.
* **O que destacar no print:** A tabela do espelho individual de jornada com o botão `📄 Extrair Relatório [RF17]`.

---

### [RF18] O funcionário administrativo deve desativar o usuário no sistema após a confirmação do desligamento
* **Perfil:** `Auxiliar Administrativo / RH`
* **Descrição Oficial do PDF:** Em caso de rescisão contratual ou desligamento pelo departamento de RH, o funcionário administrativo tem como requisito a desativação imediata das credenciais do usuário desligado para garantir a integridade das informações.
* **Onde encontrar na tela:** Tabela "Relação de Colaboradores Cadastrados [RF15 / RF18]" (coluna esquerda do Auxiliar Adm).
* **O que fazer para o print:**
  1. Na linha de qualquer colaborador, localize o botão vermelho **"Desativar"**.
  2. Ao clicar, o sistema inativa imediatamente o cadastro do colaborador, bloqueando seu login e revogando seus acessos ao sistema conforme RN06.
* **O que destacar no print:** A coluna com o botão de desativação e o badge `[RF18: Desativação de Usuário]`.

---

### [RF19] Realizar fechamento de caixa diário
* **Perfil:** `Gerente Geral` (ou `Operador de Caixa`)
* **Descrição Oficial do PDF:** O sistema deve permitir que o funcionário do caixa e a gerência executem a rotina de fechamento de turno ou encerramento do dia, consolidando e somando pagamentos recebidos (dinheiro, cheque, cartões e a prazo), emitindo resumo na tela para confrontar com o dinheiro físico na gaveta.
* **Onde encontrar na tela:** Seção "Controle e Homologação de Fechamento de Caixa [RN16 / RF19]" na tela do Gerente.
* **O que fazer para o print:**
  1. No painel do Gerente, role até o fechamento de caixa.
  2. Veja o resumo consolidado dos totais apurados por forma de pagamento (Dinheiro, Débito, Crédito, Cheque e A Prazo).
  3. Localize o botão verde **"Homologar Fechamento do Caixa [RN16 / RF19]"**.
* **O que destacar no print:** A tabela de confronto financeiro dos totais do caixa por modalidade de pagamento e o botão de homologação.

---

### [RF20] Manter relatórios gerenciais de vendas e estoque
* **Perfil:** `Gerente Geral`
* **Descrição Oficial do PDF:** Área exclusiva para a gerência emitir relatórios administrativos detalhados contendo volume de combustíveis vendidos, faturamento da loja, relação de clientes com dívidas pendentes e posição atual do estoque (percentual dos tanques e produtos na loja), permitindo filtrar por períodos específicos (diário, semanal ou mensal).
* **Onde encontrar na tela:** Topo do Painel de Controle do Gerente (Barra de Filtro e Cards Executivos Clicáveis).
* **O que fazer para o print:**
  1. No topo do Gerente, visualize a barra de filtros: **"📅 Diário"**, **"📅 Semanal"** e **"📅 Mensal"** [RF20].
  2. Clique nos botões para demonstrar o recálculo dos indicadores para cada período.
  3. Clique no Card **"FATURAMENTO PISTA"** ou **"FATURAMENTO LOJA"** para expandir a auditoria detalhada que lista frentista, operador de caixa, combustível/produto, litros e valores de cada venda.
* **O que destacar no print:** A barra de filtros de período (Diário, Semanal, Mensal), os 4 cards executivos e a tabela de auditoria analítica expandida.

---

## 📊 Matriz Resumo de Referência para o Relatório

| Requisito | Nome do Requisito Oficial (PDF) | Tela / Perfil | Componente Principal para Print |
| :---: | :--- | :---: | :--- |
| **RF01** | Gerenciar bombas e abastecimento | **Frentista** | Display LCD das 6 bombas + Fluxo contínuo e pulsos |
| **RF02** | Gerenciar vendas da conveniência | **Caixa** | Catálogo de produtos + Carrinho com totalizadores |
| **RF03** | Emitir nota fiscal | **Caixa** | Botão `📄 Emitir NF-e [RF03]` no histórico de vendas |
| **RF04** | Controlar pagamentos a prazo | **Caixa** | Botão `📑 A Prazo [RF04]` + Bloqueio para inadimplentes |
| **RF05** | Gerar boletos para empresas | **Gerente** | Tabela corporativa + Botão `📑 Gerar Boleto Mensal` |
| **RF06** | Gerenciar clientes | **Caixa / Frentista**| Botão `➕ Novo Cliente [RF06]` + Fuga pela placa |
| **RF07** | Controlar acúmulo de gastos | **Caixa / Gerente** | Card do cliente com `Total gasto acumulado [RF07]` |
| **RF08** | Monitorar nível dos tanques | **Gerente / Adm** | Painel volumétrico com percentual dos 3 tanques |
| **RF09** | Gerenciar compras combustível | **Aux. Adm** | Formulário de descarga com distribuidora Petrobras |
| **RF10** | Emitir alerta de nível baixo comb. | **Gerente** | Banner de alerta (< 15% reposição / < 4% bloqueio) |
| **RF11** | Gerenciar produtos conveniência | **Gerente** | CRUD de produtos com preço, estoque e categoria |
| **RF12** | Alerta de estoque mínimo produtos | **Gerente** | Card 4 + Modal com produtos abaixo do mínimo |
| **RF13** | Gerenciar fornecedores conveniência | **Aux. Adm** | Formulário com CNPJ + Tabela de fornecedores |
| **RF14** | Gerenciar compras produtos loja | **Aux. Adm** | Seção de Aquisição de Mercadorias para a Loja |
| **RF15** | Gerenciar funcionários | **Aux. Adm** | Formulário de admissão (piso salarial) + Colaboradores |
| **RF16** | Login com usuário e senha | **Login** | Tela de Login com seleção de usuário e senha |
| **RF17** | Relatório de horas trabalhadas | **Aux. Adm** | Botão `📄 Extrair Relatório [RF17]` + Espelho de horas |
| **RF18** | Desativação de funcionário | **Aux. Adm** | Botão vermelho `Desativar [RF18]` na lista de RH |
| **RF19** | Fechamento de caixa diário | **Gerente / Caixa** | Confronto financeiro por meio de pagamento |
| **RF20** | Relatórios gerenciais e estoque | **Gerente** | Filtros Diário/Semanal/Mensal + Auditoria clicável |
