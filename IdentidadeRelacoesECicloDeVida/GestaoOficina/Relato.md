# Relato Técnico — Projeto 2: GestaoOficina

## 1. Modelagem Prévia

1. **Problema e limites:** O sistema controla ordens de serviço (OS) de uma oficina mecânica, associando o cliente atendido, os equipamentos utilizados e os serviços executados pelos mecânicos. Não inclui controle de estoque de peças nem faturamento fiscal.
2. **Classes principais:**
   - `OrdemServico`: gerencia a OS, os equipamentos alocados e as execuções de serviço.
   - `Cliente`: pessoa atendida pela OS.
   - `Mecanico`: profissional responsável pelas execuções, identificado por `CodigoRegistro`.
   - `Equipamento`: ferramenta ou aparelho da oficina utilizado no atendimento.
   - `ExecucaoServico`: classe associativa que representa a execução de um serviço por um mecânico dentro da OS.
3. **Identidade lógica:** `Mecanico` implementa `Equals(object?)` e `GetHashCode()` baseados em `CodigoRegistro` (ignorando maiúsculas/minúsculas), pois o código funcional identifica o profissional mesmo que seu nome seja atualizado.
4. **Mutabilidade do identificador:** `CodigoRegistro` é imutável (apenas `get`), garantindo estabilidade de identidade e hash durante toda a vida do objeto.
5. **Relação com dados próprios:** `ExecucaoServico` liga `OrdemServico` e `Mecanico`. `Descricao`, `Valor` e `Status` pertencem àquela execução específica, e não ao mecânico ou à OS isoladamente.
6. **Estados e comportamentos da relação:** `StatusExecucao` (`Agendada`, `Concluida`, `Cancelada`). Inicia como `Agendada`, podendo ser concluída (`Concluir()`) ou cancelada (`Cancelar()`). **Transição proibida:** concluir uma execução já `Cancelada`.
7. **Associação, agregação e composição:**
   - **Associação (`OrdemServico` → `Cliente`):** A OS conhece o cliente atendido, mas o cliente existe independentemente da OS.
   - **Agregação (`OrdemServico` → `Equipamento`):** Equipamentos são recursos da oficina que podem ser alocados em várias OS e continuam existindo após o fim de qualquer atendimento.
   - **Composição (`OrdemServico` → `ExecucaoServico`):** Cada execução nasce na OS e perde sentido operacional se a OS for cancelada.
8. **Multiplicidades:** `OrdemServico` 0..* — 1 `Cliente`; `OrdemServico` 0..* — 0..* `Equipamento`; `OrdemServico` 1 — 0..* `ExecucaoServico`; `ExecucaoServico` 0..* — 1 `Mecanico`.
9. **Ciclo de vida:** `Program.cs` cria `Cliente`, `Mecanico` e `Equipamento`. `OrdemServico` cria e controla `ExecucaoServico`. Ao chamar `CancelarOrdem()`, execuções agendadas são canceladas, mas `Cliente`, `Mecanico` e `Equipamento` continuam válidos.
10. **Regras protegidas:** Bloqueio de nulos e textos vazios, valor de serviço maior que zero, impedimento de alocar o mesmo mecânico duas vezes na mesma OS e proibição de concluir serviço cancelado.
11. **Recortes de escopo:** Não foram modelados veículos do cliente, peças de reposição nem histórico de pagamentos para manter o foco nos requisitos de identidade e ciclo de vida.

### Diagrama de Classes (Mermaid)
```mermaid
classDiagram
    OrdemServico "0..*" --> "1" Cliente : associação
    OrdemServico "0..*" o-- "0..*" Equipamento : agregação
    OrdemServico "1" *-- "0..*" ExecucaoServico : composição
    ExecucaoServico "0..*" --> "1" Mecanico : associação


### **2. Erro de Compilação Provocado (Erro 2 de 2 do trabalho)**
    Trecho de código que causou o erro (em Program.cs):
        ```os.Equipamentos.Add(elevador);```

    Mensagem do compilador:
    error CS1061: 'IReadOnlyCollection<Equipamento>' does not contain a definition for 'Add' and no accessible extension method 'Add' accepting a first argument of type 'IReadOnlyCollection<Equipamento>' could be found

    Interpretação: Como Equipamentos é exposto como IReadOnlyCollection<Equipamento>, o compilador impede qualquer código externo de modificar a lista diretamente.

    Correção e aprendizado: Substituí pela chamada ao método de intenção os.AlocarEquipamento(elevador). Isso comprovou na prática como o encapsulamento de coleções protege as invariantes da classe OrdemServico.


### **3. Relato Curto**

    1. Identidade vs Referência: Mecanico usa CodigoRegistro (case-insensitive) como identidade. ReferenceEquals(mec1, mec1Copia) retorna false (instâncias diferentes), enquanto mec1.Equals(mec1Copia) retorna true.

    2. Consistência entre Equals e GetHashCode: Ambos utilizam OrdinalIgnoreCase sobre CodigoRegistro. Comprovei verificando que mec1.GetHashCode() == mec1Copia.GetHashCode() resulta em true.

    3. Regra de duplicidade: OrdemServico.AdicionarExecucao usa Any(e => e.Mecanico.Equals(mecanico)) para impedir que o mesmo mecânico seja alocado duas vezes na mesma OS, alinhada à identidade do mecânico.

    4. Classe associativa: ExecucaoServico foi criada para concentrar Descricao, Valor e Status de cada serviço realizado dentro de uma OS.

    5. Estados e transição proibida: Concluir() e Cancelar() alteram StatusExecucao. Concluir após cancelar foi proibido para manter coerência com o encerramento do serviço.

    6. Associação, agregação e composição: OrdemServico → Cliente (associação), OrdemServico → Equipamento (agregação de recursos reutilizáveis) e OrdemServico → ExecucaoServico (composição dependente da OS).

    7. Encerramento do todo: CancelarOrdem() marca a OS como cancelada e cancela as execuções agendadas. Cliente, Mecanico e Equipamento permanecem intactos.

    8. Responsabilidades: Regras de negócio, validações e controle de listas ficam nas classes em Modelo/. Program.cs apenas orquestra a demonstração dos cenários.

    9. Erro corrigido no desenvolvimento: Tentativa de chamar .Add() diretamente na propriedade somente leitura os.Equipamentos (erro CS1061), corrigida usando os.AlocarEquipamento().

    10. Melhoria para uma segunda versão: Permitir que o mesmo mecânico execute múltiplos serviços distintos na mesma OS, validando duplicidade pela combinação de mecânico + descrição do serviço.