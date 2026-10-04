# Relato
Projeto: ClubeLeitura

## Modelagem
1. **Problema:** Gerir o progresso de leitura dos membros de um clube, garantindo que a discussão de uma obra só ocorra após a sua leitura integral.
2. **Entidade principal:** `LivroDiscussao`.
3. **Dados do estado:** Título da obra, número total de páginas, quantidade de páginas já lidas e o estado de conclusão da discussão.
4. **Estados inválidos:** Um livro sem título, com o total de páginas negativo ou igual a zero, possuir um registo de páginas lidas superior ao total da obra, ou concluir a discussão antes de atingir os 100% de leitura.
5. **Comportamentos:** Avançar a leitura (incremento de páginas) e concluir a discussão (validação do término da leitura).
6. **O que não foi representado:** Não representei dados do leitor, pois o foco deste contexto é isolar as regras de validação do avanço de páginas, mantendo a classe estritamente em torno do seu próprio estado.

## Relato curto
**1 - Qual contexto você escolheu e qual problema o programa representa?**
Escolhi o contexto de um clube de leitura. O problema representado é a necessidade de bloquear a discussão de uma obra de ficção até que todas as páginas tenham sido validadas e registadas como lidas pelo utilizador.

**2 - Quais dados formam o estado da sua entidade principal?**
O estado da entidade `LivroDiscussao` é formado pelo `Titulo` (texto), `TotalPaginas` (inteiro), `PaginasLidas` (inteiro) e `DiscussaoConcluida` (booleano).

**3 - Qual regra impede que um objeto fique inválido?**
As validações implementadas no construtor e nos métodos. Por exemplo, o método `AvancarLeitura` avalia os parâmetros e lança uma exceção (`ArgumentException`) se a quantidade de páginas submetida ultrapassar o limite real da obra.

**4 - Por que o construtor exige determinados valores?**
Porque a entidade não faria sentido existir no sistema sem um nome e um tamanho definido. Exigir o título e o total de páginas no construtor garante que o objeto nasce num estado coerente, impedindo a criação de instâncias incompletas.

**5 - Qual propriedade possui escrita controlada? Por que ela não tem set público?**
A propriedade `PaginasLidas` possui escrita controlada através de `private set`. Ela não tem um `set` público para evitar que o código externo no `Main` lhe atribua um valor inválido (como um número negativo), forçando a alteração exclusivamente através do comportamento definido no método `AvancarLeitura()`.

**6 - Que responsabilidade ficou dentro da classe e qual ficou em Main?**
A classe responsabilizou-se por proteger o seu estado interno e aplicar as lógicas do domínio. O Main assumiu a responsabilidade de interagir com o utilizador via terminal, converter os dados de entrada, capturar as exceções (usando `try-catch`) e apresentar os erros.

**7 - O que aconteceu quando você tentou executar uma operação inválida?**
Ao tentar registar 200 páginas num livro que possuía apenas 150, o método recusou a ação lançando uma exceção. O fluxo foi desviado para o bloco `catch` no `Main`, que imprimiu a mensagem de aviso e permitiu que o programa continuasse sem quebrar abruptamente.

**8 - O que você mudaria se fosse desenvolver uma segunda versão?**
Numa segunda versão, estudaria formas de organizar o catálogo e incluiria os perfis dos leitores. 

## Erros de compilação (se ocorreram neste projeto)[cite: 6]
### Erro 1
- **Código que causou:** `livro1.PaginasLidas = 50;`
- **Mensagem do compilador:** `error CS0272: The property or indexer 'LivroDiscussao.PaginasLidas' cannot be used in this context because the set accessor is inaccessible.`
- **Interpretação:** O compilador indica que a propriedade tem um nível de proteção (`private set`) que impede a sua atribuição direta a partir do código exterior (o `Main`).
- **Correção:** Apaguei a atribuição direta e utilizei a operação correta através do método: `livro1.AvancarLeitura(50);`.

### Erro 2
- **Código que causou:** `LivroDiscussao livro = new LivroDiscussao();`
- **Mensagem do compilador:** `error CS7036: There is no argument given that corresponds to the required parameter 'titulo' of 'LivroDiscussao.LivroDiscussao(string, int)'.`
- **Interpretação:** O compilador exige os dados obrigatórios definidos no construtor para instanciar o objeto, mas nenhum argumento foi enviado.
- **Correção:** Alterei a instanciação para enviar os parâmetros corretos exigidos: `LivroDiscussao livro = new LivroDiscussao("Fundação", 255);`.


**9 - Qual erro de compilação ajudou mais a compreender private, private set ou o uso de métodos? Explique.**
O Erro 1 (CS0272) foi o mais clarificador. Ao tentar alterar a propriedade diretamente no `Main`, o compilador bloqueou a operação, demonstrando na prática o funcionamento do encapsulamento. Percebi que o `private set` retira o controlo de quem usa a classe, tornando os métodos indispensáveis como mediadores para validar os dados antes que o estado sofra qualquer alteração.
========================================================================



