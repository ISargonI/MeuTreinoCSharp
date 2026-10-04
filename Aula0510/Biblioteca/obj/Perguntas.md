1. Qual é a multiplicidade entre Leitor e Livro neste problema?
A multiplicidade é de 1 para 0..3. Um Leitor pode possuir de zero a no máximo três objetos Livro em sua coleção de empréstimos simultâneos.   

2. Por que a coleção pode começar vazia?
Porque uma das regras de domínio especifica claramente que um leitor pode ser cadastrado no sistema sem possuir nenhum livro emprestado naquele momento.   

3. Quem deve controlar o limite de três empréstimos?
O próprio objeto Leitor. Como o Leitor é a classe que controla a sua coleção de livros emprestados e tem autoridade sobre as mudanças de seu estado interno, a validação dessa invariante de limite pertence a ele.  

4. Qual informação pode ser usada como identidade de Livro? Justifique.
O ISBN. O ISBN é um atributo natural único de identificação global para publicações. Ao usar o ISBN na reescrita dos métodos Equals e GetHashCode, o sistema consegue detectar de forma confiável tentativas de adicionar o mesmo livro duas vezes à coleção, cumprindo a regra de duplicidade.

5. Por que IReadOnlyCollection não significa que o Leitor perdeu a capacidade de alterar sua coleção?
Porque a interface IReadOnlyCollection restringe apenas quem consome a coleção externamente, impedindo o uso direto de métodos como Add ou Remove pelo código externo. Internamente, o Leitor continua mantendo a referência privada para a lista mutável (como um List<Livro>) e tem total capacidade de alterá-la por meio de seus próprios métodos de domínio, como EmprestarLivro e DevolverLivro.