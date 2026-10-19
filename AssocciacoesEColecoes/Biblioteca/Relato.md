1. Como as classes e os objetos se relacionam?   
A classe Leitor contém uma lista de objetos Livro. A relação representa o ato de um utilizador da biblioteca ter livros na sua posse (empréstimo).

2. Que multiplicidade foi definida e por que ela representa o domínio?  
A multiplicidade é de 0..3. Representa o facto de que um leitor pode estar registado sem ter qualquer livro consigo (0), mas, pelas regras do domínio da biblioteca física, não pode levar mais do que 3 livros simultaneamente para casa.

3. Quais dados foram representados por referência, evitando duplicação?   
Os dados do título e do ISBN pertencem exclusivamente à classe Livro. O objeto Leitor apenas armazena a referência aos livros em vez de criar listas de strings para guardar os nomes, evitando a redundância.

4. Como a classe responsável protege a coleção?
A coleção _emprestimos no leitor está definida como privada e é apenas exposta ao exterior através de IReadOnlyCollection. Modificações (empréstimos e devoluções) só ocorrem por métodos do próprio leitor.

5. Como a identidade escolhida impede inclusões duplicadas?   
A identidade do livro é o ISBN. Ao efetuar um empréstimo, o método percorre a lista para garantir que o leitor não está a tentar requisitar outra cópia do mesmo ISBN que já tem na sua posse.

6. O que acontece quando a aplicação tenta adicionar um objeto inválido?
A classe Leitor levanta uma exceção (como InvalidOperationException ou ArgumentNullException), barrando a entrada. O fluxo normal é interrompido e a aplicação principal (Main) trata o erro com um bloco catch.

7. Qual operação de inclusão, remoção ou listagem foi mais desafiadora?
Controlar a capacidade máxima no código foi interessante, pois obrigou a verificar a contagem da coleção .Count antes de inserir.

8. O que você mudaria em uma segunda versão?
Poderia ser criado uma classe Emprestimo contendo a data de requisição e a data de devolução esperada, ligando assim o Leitor ao Livro com limites de prazo.


Registo do Erro de Compilação Provocado (1 de 2)  
O trecho de código que causou o erro:leitor.Emprestimos.Add(livro1); 
(colocado no Program.cs)A mensagem do compilador:error CS1061: 'IReadOnlyCollection<Livro>' does not contain a definition for 'Add' and no accessible extension method 'Add' accepting a first argument of type 'IReadOnlyCollection<Livro>' could be found...

O compilador avisa que a propriedade Emprestimos, sendo do tipo IReadOnlyCollection, é estruturalmente incapaz de aceitar novos itens através do método genérico Add(), que é inexistente nesta interface.
Como foi corrigido o problema:
Alterando a coleção de forma correta, utilizando o método público RequisitarLivro(livro1) exposto pelo objeto Leitor para pedir que realizasse a adição sob o seu próprio controle.