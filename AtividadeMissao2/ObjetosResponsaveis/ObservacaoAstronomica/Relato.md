# Relato
Projeto: ObservacaoAstronomica

## Modelagem
1. **Problema:** Registrar e acumular o tempo de observação de satélites no céu noturno, validando as coordenadas celestes e impedindo edições após o encerramento do rastreio.
2. **Entidade principal:** `RegistroSatelite`.
3. **Dados do estado:** Nome do satélite, azimute (graus), minutos observados e o status indicando se o rastreio está ativo.
4. **Estados inválidos:** Um satélite sem nome, um azimute negativo ou superior a 360 graus, tempo de observação negativo ou tentar adicionar tempo a um rastreio já encerrado.
5. **Comportamentos:** Adicionar tempo de visibilidade e encerrar o rastreio definitivamente.
6. **O que não foi representado:** Não representei dados do telescópio ou do usuário do aplicativo, mantendo a classe focada apenas nos dados astronômicos do objeto observado.

## Relato curto
**1 - Qual contexto você escolheu e qual problema o programa representa?**
Escolhi o contexto de observação astronômica e rastreamento de satélites no céu noturno. O problema é validar coordenadas e acumular o tempo de visibilidade sem permitir dados fisicamente impossíveis.

**2 - Quais dados formam o estado da sua entidade principal?**
O estado é formado por `Nome` (string), `Azimute` (double), `MinutosObservados` (inteiro) e `RastreioAtivo` (booleano).

**3 - Qual regra impede que um objeto fique inválido?**
O construtor e os métodos verificam se o azimute está entre 0 e 360 graus e se os minutos adicionados são valores positivos, lançando `ArgumentOutOfRangeException` ou `ArgumentException` caso as leis da geometria e do tempo sejam desrespeitadas.

**4 - Por que o construtor exige determinados valores?**
Porque um registro num mapa celestial precisa obrigatoriamente de um alvo (como a ISS ou Starlink) e de uma posição inicial apontada no céu (azimute) para existir concretamente no aplicativo.

**5 - Qual propriedade possui escrita controlada? Por que ela não tem set público?**
A propriedade `MinutosObservados` possui `private set`. Sem isso, o `Main` poderia sobrescrever o tempo total com um número negativo ou zerar a contagem indevidamente a qualquer momento. O controle de acréscimo é feito apenas via `AdicionarTempoVisibilidade()`.

**6 - Que responsabilidade ficou dentro da classe e qual ficou em Main?**
A classe assumiu a validação das regras físicas e lógicas (como travar o cronômetro). O `Main` assumiu o uso de laços de repetição `while` para insistir na digitação correta por parte do usuário e formatar a exibição das exceções.

**7 - O que aconteceu quando você tentou executar uma operação inválida?**
Tentei adicionar tempo de observação ao telescópio Hubble, cujo rastreio já havia sido marcado como encerrado. A classe lançou uma `InvalidOperationException` informando o bloqueio, e o `Main` capturou o erro elegantemente sem fechar o console.

**8 - O que você mudaria se fosse desenvolver uma segunda versão?**
Incluiria o cálculo de elevação (altitude) em graus para complementar o azimute. Além disso, utilizaria modelagem Entidade-Relacionamento e consultas SQL para persistir o histórico das noites de observação em um banco de dados, em vez de guardar os dados apenas em memória durante a execução do programa.