# Relato Técnico — Projeto 3: CooperativaEntregas

## 1. Modelagem Prévia

1. **Problema e limites:** O sistema organiza rotas de entrega de uma cooperativa, associando o coordenador responsável, os veículos da frota e os registros de entrega atribuídos aos entregadores. Não inclui roteirização por GPS, pagamento de comissões nem persistência em banco de dados.
2. **Classes principais:**
   - `RotaEntrega`: coordena a rota, os veículos vinculados e os registros de entrega.
   - `Coordenador`: responsável operacional pela rota.
   - `Entregador`: cooperado identificado por um `Guid Id` imutável.
   - `Veiculo`: bem da frota que pode ser alocado em diferentes rotas.
   - `RegistroEntrega`: classe associativa que vincula `RotaEntrega` e `Entregador`.
3. **Identidade lógica:** `Entregador` implementa `Equals(object?)` e `GetHashCode()` baseados em `Id` (`Guid`), garantindo uma identidade permanente mesmo que nome ou documento sejam corrigidos.
4. **Mutabilidade do identificador:** `Id` é imutável (somente `get`), definido no construtor. Para evitar cadastros duplicados com o mesmo `Documento` (que pode ser corrigido), a classe `RotaEntrega` aplica uma regra de negócio separada usando `Any()`.
5. **Relação com dados próprios:** `RegistroEntrega` liga `RotaEntrega` e `Entregador`. `EnderecoDestino`, `TaxaEntrega` e `Status` pertencem àquela entrega específica, e não ao entregador ou à rota isoladamente.
6. **Estados e comportamentos da relação:** `StatusEntrega` (`Pendente`, `Entregue`, `Cancelada`). Inicia como `Pendente`, podendo ir para `Entregue` (`MarcarComoEntregue()`) ou `Cancelada` (`Cancelar()`). **Transição proibida:** marcar como `Entregue` após `Cancelada`.
7. **Associação, agregação e composição:**
   - **Associação (`RotaEntrega` → `Coordenador`):** A rota conhece seu coordenador, que existe de forma independente.
   - **Agregação (`RotaEntrega` → `Veiculo`):** Veículos pertencem à frota e podem ser reutilizados em múltiplas rotas mesmo após o encerramento de uma rota específica.
   - **Composição (`RotaEntrega` → `RegistroEntrega`):** Cada registro de entrega pertence exclusivamente àquela rota e perde validade operacional quando a rota é encerrada.
8. **Multiplicidades:** `RotaEntrega` 0..* — 1 `Coordenador`; `RotaEntrega` 0..* — 0..* `Veiculo`; `RotaEntrega` 1 — 0..* `RegistroEntrega`; `RegistroEntrega` 0..* — 1 `Entregador`.
9. **Ciclo de vida:** `Program.cs` cria `Coordenador`, `Entregador` e `Veiculo`. `RotaEntrega` cria e controla `RegistroEntrega`. Ao chamar `EncerrarRota()`, entregas pendentes são canceladas, mas `Coordenador`, `Entregador` e `Veiculo` continuam válidos.
10. **Regras protegidas:** Bloqueio de nulos e textos vazios, taxa de entrega positiva, bloqueio de duplicidade por `Id` ou `Documento` na mesma rota e proibição de concluir entrega cancelada.
11. **Recortes de escopo:** Não foram modelados clientes destinatários, peso dos pacotes nem horários estimados para manter o foco nos conceitos de identidade, classe associativa e ciclo de vida.

### Diagrama de Classes (Mermaid)
```mermaid
classDiagram
    RotaEntrega "0..*" --> "1" Coordenador : associação
    RotaEntrega "0..*" o-- "0..*" Veiculo : agregação
    RotaEntrega "1" *-- "0..*" RegistroEntrega : composição
    RegistroEntrega "0..*" --> "1" Entregador : associação



## **2. Relato Curto**
    1. Identidade vs Referência: Entregador usa Guid Id como identidade. ReferenceEquals(ent1, ent1MesmaEntidade) retorna false (instâncias distintas), mas ent1.Equals(ent1MesmaEntidade) retorna true por compartilharem o mesmo Id.

    2. Consistência entre Equals e GetHashCode: Ambos usam Id. Comprovei verificando ent1.GetHashCode() == ent1MesmaEntidade.GetHashCode() como true.

    3. Regra de duplicidade: Diferente dos projetos anteriores, aqui identidade (Id) e duplicidade (Documento) são regras separadas. RotaEntrega.RegistrarEntrega usa Any() para impedir dois registros com o mesmo Id ou o mesmo Documento, mesmo que tenham Guid diferentes.

    4. Classe associativa: RegistroEntrega foi criada para armazenar EnderecoDestino, TaxaEntrega e Status, que pertencem à ocorrência da entrega naquela rota.

    5. Estados e transição proibida: MarcarComoEntregue() e Cancelar() alteram o estado. Concluir uma entrega cancelada foi proibido para evitar inconsistência operacional.

    6. Associação, agregação e composição: RotaEntrega → Coordenador (associação), RotaEntrega → Veiculo (agregação de ativos reutilizáveis) e RotaEntrega → RegistroEntrega (composição com dependência de existência).

    7. Encerramento do todo: EncerrarRota() marca a rota como encerrada e cancela registros pendentes. Coordenador, Entregador e Veiculo permanecem disponíveis.

    8. Responsabilidades: Validações, proteção de coleções e transições de estado estão em Modelo/. Program.cs apenas executa os cenários de demonstração.

    9. Erro corrigido no desenvolvimento: Inicialmente a verificação de duplicidade considerava apenas Equals() (por Id), o que permitiria cadastrar o mesmo documento com outro Guid; corrigi adicionando a comparação explícita de Documento via Any().

    10. Melhoria para uma segunda versão: Permitir que o mesmo entregador faça múltiplas entregas na mesma rota desde que os endereços de destino sejam diferentes.