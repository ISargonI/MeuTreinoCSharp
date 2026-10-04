1. Por que Projeto não deveria alterar diretamente a situação interna de Tarefa?
Porque a Tarefa é a dona de seus próprios dados e estado. Se o Projeto alterasse a situação diretamente, ele violaria o encapsulamento. A evolução do estado (de pendente para concluída) é um comportamento que pertence ao domínio da própria Tarefa.   

2. Por que Projeto ainda precisa consultar a situação para decidir sobre a remoção?
Porque a regra que impede a remoção de uma tarefa concluída da coleção é uma responsabilidade do Projeto. Como o Projeto controla os elementos que entram e saem de sua lista, ele deve consultar a Tarefa para saber se a condição para mantê-la na lista (estar concluída) se aplica antes de executar a remoção.   

3. Quem protege a identidade e os dados da Tarefa?
A própria classe Tarefa. Ela valida o título e o código no construtor e utiliza o código em sua reescrita de Equals e GetHashCode para proteger a própria identidade perante as regras do sistema.   

4. Quem protege as regras da relação Projeto-Tarefa?
A classe Projeto. Ela é a proprietária da coleção e restringe a visibilidade para o código externo, protegendo a inserção (impedindo códigos duplicados) e a remoção (impedindo remoção de tarefas concluídas) por meio de seus métodos de domínio.   

5. Este exercício mostra como regras do elemento e regras da coleção podem colaborar? Explique.
Sim. O exercício demonstra colaboração através da divisão clara de responsabilidades: a Tarefa fornece e controla a informação sobre o seu próprio estado (a regra do elemento diz: "eu sei dizer se estou concluída ou não"). O Projeto utiliza essa informação para proteger a composição do agrupamento (a regra da coleção diz: "eu não removo tarefas que me informam estar concluídas"). 