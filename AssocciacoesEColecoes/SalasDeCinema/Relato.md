1. Como as classes e os objetos se relacionam?   
A classe Sessao agrupa múltiplos objetos da classe Ingresso. A Sessao representa a exibição de um filme, e os objetos Ingresso representam o estado dos lugares ocupados para aquele filme específico.

2. Que multiplicidade foi definida e por que ela representa o domínio?   
A multiplicidade é 0..* (zero ou muitos), com um limite máximo definido no construtor. Isso ocorre porque uma sessão pode ser criada sem nenhum ingresso vendido (0), e pode crescer gradualmente até atingir a capacidade máxima estipulada pela sala de cinema física.

3. Quais dados foram representados por referência, evitando duplicação?   
Os detalhes do assento e o tipo do ingresso (Meia/Inteira) pertencem à classe Ingresso. A classe Sessao não duplica estes dados internamente como matrizes de strings, simplesmente detém a referência aos objetos Ingresso gerados.

4. Como a classe responsável protege a coleção?  
A coleção _ingressos foi definida como private readonly List<Ingresso>, não permitindo adições externas. Para consulta, foi exposta uma propriedade do tipo IReadOnlyCollection<Ingresso>. Qualquer inserção precisa passar pelo método VenderIngresso, que processa as regras de negócio.

5. Como a identidade escolhida impede inclusões duplicadas?   
A identidade escolhida para o ingresso foi o Assento (ex: "A1"). O método de adição varre a coleção existente; se encontrar um objeto cujo Assento seja igual ao que está a ser inserido, lança uma InvalidOperationException e recusa a operação.

6. O que acontece quando a aplicação tenta adicionar um objeto inválido?
A aplicação interrompe o método e lança excepções específicas dependendo da falha (ArgumentNullException para objetos nulos e InvalidOperationException para lotação esgotada ou assentos repetidos). O Main captura estas exceções num bloco try-catch e avisa o utilizador.

7. Qual operação de inclusão, remoção ou listagem foi mais desafiadora?  
Foi interessante garantir que a remoção valida se o ingresso realmente faz parte da lista com o método "Contains".

8. O que você mudaria em uma segunda versão?   
Acho que poderia adicionar a validação do tipo de entrada como Inteira/Meia baseada na idade.