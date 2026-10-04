1. Equipe possui a mesma multiplicidade com Líder e com Membros?
Não. A relação de Equipe com o líder tem multiplicidade exatamente um (1..1), configurando uma associação obrigatória e singular. Já a relação com os membros tem multiplicidade de zero a muitos (0..6), configurando uma coleção opcional inicialmente e com limite de capacidade.   

2. Por que a existência de uma coleção não elimina a associação simples com o líder?
Porque o líder possui uma responsabilidade de domínio distinta e estrutural dentro do sistema. O líder precisa existir obrigatoriamente desde a criação da equipe e garantir seu funcionamento, logo, ele não é apenas um item arbitrário dentro de um agrupamento.   

3. Quem deve validar nome, e-mail e especialidade de Profissional?
A própria classe Profissional. Isso garante que a validação ocorra no momento da construção do objeto, impedindo que uma instância em estado inválido seja criada e repassada para o restante do domínio.   

4. Quem deve validar duplicidade e capacidade da equipe?
A classe Equipe. Como ela é a "dona" da relação e a responsável exclusiva por alterar sua própria coleção, as invariantes que dizem respeito à composição da equipe (limite de 6 e ausência de duplicidade) devem ser verificadas em seus métodos internos antes de qualquer modificação de estado.  

5. Seria adequado expor public List Membros { get; private set; }? Explique.
Não seria adequado. Embora o private set impeça que o código externo substitua a lista inteira por uma nova instância com = new List(), expor a interface List<T> publicamente permite que qualquer programador chame equipe.Membros.Add() ou equipe.Membros.Remove(). Isso contorna completamente as validações de capacidade e duplicidade protegidas nos métodos do domínio.