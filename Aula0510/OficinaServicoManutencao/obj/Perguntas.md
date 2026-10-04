1. Por que uma coleção de Servico seria insuficiente para representar completamente este domínio?
Porque dados críticos como a "quantidade" executada e a "observação" de aplicação não pertencem ao catálogo do Servico em si, mas sim ao ato de incluir esse serviço numa ordem específica. Se tivéssemos apenas uma List<Servico>, perderíamos a capacidade de registar quantas vezes o serviço foi aplicado ou os detalhes dessa aplicação.   

2. A quem pertence a quantidade: ao Servico ou ao ItemManutencao?
Pertence ao ItemManutencao. A quantidade é uma característica da relação (quantas unidades foram pedidas nesta ordem específica) e não do serviço base.  

3. Por que observação pode ser string? neste caso?
A regra de negócio define explicitamente que a observação é opcional. Ao usar o tipo anulável string?, estamos a comunicar de forma expressa no código que a ausência de valor (nulo) é um estado válido e esperado para esta propriedade.   

4. Qual objeto deve detetar se o mesmo Servico já está presente na ordem?
A OrdemServico. Como é este objeto que detém e gere a coleção de ItemManutencao, a responsabilidade de iterar sobre os itens e garantir que dois itens referentes ao mesmo serviço não coexistam na coleção recai sobre os seus métodos de controlo.   

5. Em que sentido ItemManutencao é uma classe associativa?
É associativa porque nasce para materializar a relação muitos-para-muitos (ou neste caso, um-para-muitos detalhado) entre OrdemServico e Servico. A relação entre as duas entidades possui dados próprios (quantidade, observação) que justificam a criação de uma classe autónoma em vez de uma simples associação direta.   

6. Qual é a diferença conceitual entre OrdemServico -> ItemManutencao e ItemManutencao -> Servico?
A relação OrdemServico -> ItemManutencao descreve a gestão de uma coleção restrita e protegida, ou seja, uma associação de um-para-muitos onde a ordem compõe e controla o ciclo de vida dos seus múltiplos itens. Por outro lado, ItemManutencao -> Servico é uma associação simples e obrigatória para exatamente um objeto, onde o item apenas mantém uma referência para um catálogo externo, não o controlando nem gerindo a sua existência. 