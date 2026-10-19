1. Como as classes e os objetos se relacionam?   
A classe Medico contém uma lista de objetos Paciente. A relação representa uma fila de espera clínica, onde um médico tem vários doentes sob a sua responsabilidade num determinado turno.

2. Que multiplicidade foi definida e por que ela representa o domínio? 
A multiplicidade é 0..3. Representa que um médico pode iniciar o turno sem pacientes (0) e tem um limite máximo de fila simultâneo (neste exemplo, 3) para evitar sobrelotação no consultório.
3. Quais dados foram representados por referência, evitando duplicação?  
Os sintomas, o nome do paciente e o número de cadastro SUS não são gravados no objeto Medico em variáveis independentes. O médico apenas mantém a referência de memória para o objeto Paciente, onde esses dados residem originalmente.

4. Como a classe responsável protege a coleção?   
A coleção _filaEspera está encapsulada (private). A classe externa (Program) não consegue usar .Clear() ou .Add(). Todo o fluxo é mediado pelos métodos AgendarPaciente e AtenderPaciente, que efetuam validações antes de alterar o estado.

5. Como a identidade escolhida impede inclusões duplicadas?   
O NumeroSus foi escolhido como identificador universal. Ao inserir, a coleção é iterada. Se o número do SUS já constar na lista de espera, o sistema bloqueia a entrada, mesmo que o nome do novo objeto seja ligeiramente diferente.

6. O que acontece quando a aplicação tenta adicionar um objeto inválido?   
O fluxo de inserção é abortado imediatamente com o lançamento de uma exceção (InvalidOperationException). O paciente não é guardado na lista.

7. Qual operação de inclusão, remoção ou listagem foi mais desafiadora?   
(A tua perspetiva. Ex: Garantir que a identidade não dependia do objeto de memória, mas sim de uma propriedade string como o número do SUS, forçou-me a criar o foreach de verificação).

8. O que você mudaria em uma segunda versão?   
Poderia criar uma classe Turno para envolver a lógica e adicionaria um sistema de prioridades - pulseiras de cores - para ordenar a fila de espera em vez de usar uma simples List.

Registo do Erro de Compilação Provocado (2 de 2)  
O trecho de código que causou o erro:p1.Nome = "Carlos Almeida"; (colocado no Program.cs)
A mensagem do compilador:error CS0272: The property or indexer 'Paciente.Nome' cannot be used in this context because the set accessor is inaccessible.

O compilador bloqueou a atribuição de um novo nome ao paciente, pois a propriedade Nome possui um private set. Ela só pode ser definida internamente pela classe, neste caso, através do construtor.
Foi feito a remoção da tentativa de atribuição direta no Main.

Qual dos erros ajudou mais a compreender associações, coleções, nulidade ou acesso controlado? Explique.   
O segundo erro ajudou a compreender o acesso controlado. Ele demonstrou na prática que usar 'private set' blinda o estado do objeto. Fora da classe, os dados ficam como leitura, protegendo a integridade da aplicação contra alterações acidentais feitas por outras partes do código. 