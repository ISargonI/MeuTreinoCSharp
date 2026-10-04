1. A capacidade física de uma sala e o limite de reservas deste exercício são necessariamente a mesma coisa? Considere apenas as regras fornecidas.
Não. O exercício foca na "capacidade máxima de reservas" configurada especificamente para a sessão. Esse valor é uma regra de negócio abstrata que pode, por motivos variados (como manutenção de poltronas ou restrições de público), ser menor que o total de poltronas físicas construídas na sala.   

2. Por que a verificação de capacidade deve ocorrer antes do Add?
Para manter a validade do estado do objeto e garantir suas invariantes. Se a sessão realizasse o Add para depois conferir se ultrapassou o limite, o objeto Sessao existiria em um estado inválido e inconsistente na memória durante esse intervalo de tempo.   

3. Qual objeto tem autoridade para cancelar uma reserva?
A classe Sessao. A regra de domínio é clara ao afirmar que a ação de cancelar deve ocorrer por um método pertencente à Sessao, garantindo que o objeto responsável pela coleção mantenha o controle absoluto sobre inclusões e remoções.   

4. O que aconteceria se o Program.cs pudesse executar Clear() diretamente?
O Program.cs poderia apagar toda a lista de assentos reservados instantaneamente sem invocar o comportamento apropriado do domínio (CancelarReserva). Isso ignoraria possíveis regras complementares (como gerar logs, estornar pagamentos, ou emitir avisos) e violaria o encapsulamento, removendo o controle de estado da Sessao.   

5. Qual multiplicidade melhor descreve Sessao -> Assento reservado?
A multiplicidade é 0..* (ou mais precisamente, 0..CapacidadeMaxima), uma vez que uma sessão de cinema pode começar vazia (sem qualquer assento reservado) e acumular múltiplas reservas até atingir seu limite configurado. 