1. Qual problema Equals e GetHashCode ajudam a resolver neste exercício?
Eles resolvem o problema da verificação de duplicidade de dispositivos lógicos. 
Ao reescrever esses métodos utilizando o identificador, o sistema entende que duas 
instâncias na memória com o mesmo ID representam o mesmo aparelho, acionando as validações
corretas do método Contains da lista.   

2. Por que comparar apenas referências de objetos poderia não ser suficiente?
Porque o código externo poderia recriar um objeto Dispositivo utilizando o mesmo identificador
 de hardware (como um endereço MAC), resultando em um novo endereço de memória. 
 Se a conta avaliasse apenas referências de memória, ela permitiria autorizar o mesmo 
 aparelho físico duas vezes, quebrando a regra de domínio.   

3. O limite de cinco dispositivos é responsabilidade de Dispositivo ou ContaUsuario?
Da ContaUsuario. A conta é o objeto responsável por manter a associação um-para-muitos e 
gerenciar sua coleção encapsulada, portanto, é ela quem detém a autoridade e a 
responsabilidade por aplicar o limite máximo.   

4. null representa um dispositivo autorizado válido?
Não. Um valor null não possui identificador, nome ou sistema operacional válidos, violando 
a regra estrutural da classe Dispositivo. Além disso, ele não representa nenhum objeto real 
no domínio.   


5. Qual a diferença entre proteger a referência da lista e proteger seu conteúdo?
Proteger a referência (usando readonly ou um setter privado) garante apenas que o campo 
da coleção não seja sobrescrito por uma nova lista (= new List()). Proteger o conteúdo
 (encapsulando-a e retornando um tipo como IReadOnlyCollection) impede que o código externo
  utilize comandos como .Add(), .Remove() ou .Clear() diretamente na coleção existente, 
  forçando o uso dos métodos de domínio da classe controladora. 