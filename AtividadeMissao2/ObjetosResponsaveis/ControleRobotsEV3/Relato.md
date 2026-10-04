# Relato
Projeto: ControleRobotsEV3

## Modelagem
1. **Problema:** Gerir a utilização de kits de robótica em minicursos, garantindo que não são emprestados sem bateria suficiente e que não ocorrem devoluções de kits que não estavam em uso.
2. **Entidade principal:** `KitLegoEV3`.
3. **Dados do estado:** Identificação do kit, nível de bateria (percentagem) e o estado de uso (booleano).
4. **Estados inválidos:** Um kit sem identificação, bateria fora do intervalo 0-100%, tentar emprestar um kit já em uso ou tentar devolver um kit que já se encontra guardado no laboratório.
5. **Comportamentos:** Emprestar para o minicurso (altera o estado para em uso após verificar a bateria) e devolver (regista a nova bateria e liberta o equipamento).
6. **O que não foi representado:** Não representei a que grupo de estudantes o kit foi emprestado, focando apenas na viabilidade física do equipamento (carga e disponibilidade).

## Relato curto
**1 - Qual contexto você escolheu e qual problema o programa representa?**
Escolhi o contexto de oficinas de robótica. O problema é impedir o uso de kits descarregados e manter um inventário fiável de quais robôs estão efetivamente a ser operados na sala de aula.

**2 - Quais dados formam o estado da sua entidade principal?**
`Identificacao` (string), `NivelBateria` (inteiro) e `EmUso` (booleano).

**3 - Qual regra impede que um objeto fique inválido?**
O método `Devolver(int bateriaRestante)` valida se o parâmetro numérico enviado está entre 0 e 100, lançando um `ArgumentOutOfRangeException` se o valor não corresponder à realidade física de uma bateria.

**4 - Por que o construtor exige determinados valores?**
O construtor exige o `identificacao` porque é impossível rastrear um equipamento sem nome. Ao mesmo tempo, ele assume inteligentemente (hardcoded) que um kit recém-cadastrado tem 100% de bateria e está disponível (`EmUso = false`), garantindo a consistência desde o nascimento da instância.

**5 - Qual propriedade possui escrita controlada? Por que ela não tem set público?**
A propriedade `NivelBateria` possui `private set`. Se tivesse um set público, o `Main` poderia colocar a bateria a 500% ou -50%, quebrando completamente as regras do domínio. A escrita é feita exclusivamente pelo método `Devolver()`.

**6 - Que responsabilidade ficou dentro da classe e qual ficou em Main?**
A classe `KitLegoEV3` ficou com a responsabilidade de aplicar as regras de negócio de empréstimo. O `Main` assumiu a interação com o operador, a captura de inputs pelo `Console.ReadLine()` e o tratamento amigável de erros via `try-catch`.

**7 - O que aconteceu quando você tentou executar uma operação inválida?**
Tentei invocar `kit2.Devolver(90)` num kit que acabou de ser instanciado e não estava emprestado. A classe intercetou a falha lógica, lançou uma exceção (`InvalidOperationException`) que informava "O kit já consta como devolvido", abortando a alteração de estado.

**8 - O que você mudaria se fosse desenvolver uma segunda versão?**
Implementaria uma entidade para os motores e sensores individuais associados ao bloco central do EV3, talvez estruturando o sistema com uma relação de composição (onde um kit contém múltiplos componentes) para rastrear avarias em peças específicas.