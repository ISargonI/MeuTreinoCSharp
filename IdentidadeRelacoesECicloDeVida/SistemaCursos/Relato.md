# Relato Técnico — Projeto 1: SistemaCursos

## 1. Modelagem Prévia

1. **Problema e limites:** O sistema gerencia uma turma acadêmica, seu professor responsável, materiais de apoio e as matrículas dos estudantes. Não inclui gestão financeira, notas parciais ou persistência em banco de dados.
2. **Classes principais:**
   - `Turma`: coordena a turma, seus materiais e o ciclo de vida das matrículas.
   - `Professor`: docente responsável pela turma.
   - `Estudante`: aluno identificado pelo RA.
   - `MaterialApoio`: recurso didático reutilizável em várias turmas.
   - `Matricula`: classe associativa que registra a relação entre `Turma` e `Estudante`.
3. **Identidade lógica:** `Estudante` implementa `Equals(object?)` e `GetHashCode()` baseados no `Ra` (ignorando maiúsculas/minúsculas), pois o Registro Acadêmico identifica unicamente o aluno mesmo que o nome seja corrigido.
4. **Mutabilidade do identificador:** O `Ra` é imutável (somente `get`), definido no construtor. Isso evita que o código hash mude após o objeto entrar em coleções como `HashSet<T>`.
5. **Relação com dados próprios:** `Matricula` liga `Turma` e `Estudante`. Dados como `DataMatricula`, `Status` e `NotaFinal` não pertencem isoladamente ao estudante (que pode cursar várias turmas com notas diferentes) nem à turma.
6. **Estados e comportamentos da relação:** `StatusMatricula` (`Pendente`, `Ativa`, `Cancelada`). Inicia como `Pendente`. Pode ser ativada (`Ativar()`) ou cancelada (`Cancelar()`). **Transição proibida:** ativar uma matrícula já `Cancelada`.
7. **Associação, agregação e composição:**
   - **Associação (`Turma` → `Professor`):** A turma conhece o professor, mas ele possui vida independente.
   - **Agregação (`Turma` → `MaterialApoio`):** A turma reúne materiais que podem ser compartilhados entre várias turmas e continuam existindo se a turma encerrar.
   - **Composição (`Turma` → `Matricula`):** Cada matrícula nasce e só faz sentido dentro daquela turma específica.
8. **Multiplicidades:** `Turma` 1 — 1 `Professor`; `Turma` 0..* — 0..* `MaterialApoio`; `Turma` 1 — 0..* `Matricula`; `Matricula` 0..* — 1 `Estudante`.
9. **Ciclo de vida:** `Program.cs` cria `Professor`, `Estudante` e `MaterialApoio`. `Turma` cria e controla `Matricula`. Ao chamar `Turma.Encerrar()`, as matrículas pendentes são canceladas, mas `Professor`, `Estudante` e `MaterialApoio` continuam válidos.
10. **Regras protegidas:** Campos obrigatórios não vazios, bloqueio de referências nulas, bloqueio de matrícula duplicada por RA, bloqueio de nota fora de 0..10 e bloqueio de reativação de matrícula cancelada.
11. **Recortes de escopo:** Não foram modelados semestres letivos, frequência nem histórico escolar completo para manter o foco nas relações e no ciclo de vida.

### Diagrama de Classes (Mermaid)
```mermaid
classDiagram
    Turma "1" --> "1" Professor : associação
    Turma "0..*" o-- "0..*" MaterialApoio : agregação
    Turma "1" *-- "0..*" Matricula : composição
    Matricula "0..*" --> "1" Estudante : associação


## 2. Erro de Compilação Provocado (Erro 1 de 2 do trabalho)

    Trecho de código que causou o erro (em Turma.cs):
    bool jaMatriculado = _matriculas.Contains(estudante);
    Mensagem do compilador:error CS1503: Argument 1: cannot convert from 'SistemaCursos.Modelo.Estudante' to 'SistemaCursos.Modelo.Matricula'
    Interpretação: A lista _matriculas armazena objetos do tipo Matricula, e não Estudante. Portanto, Contains espera uma Matricula.
    Correção e aprendizado: Substituí por _matriculas.Any(m => m.Estudante.Equals(estudante)). 
    Aprendi que Contains() só funciona diretamente quando a coleção armazena o mesmo tipo que estamos buscando; em classes associativas, usamos Any() sobre a propriedade relacionada.


### 3. Relato Curto


    1. Identidade vs Referência: Em Estudante, a identidade é o Ra (case-insensitive). ReferenceEquals(est1, est1Copia) retorna false porque são duas instâncias criadas com new, mas est1.Equals(est1Copia) retorna true porque representam o mesmo aluno.

    2. Consistência entre Equals e GetHashCode: Coleções baseadas em hash primeiro comparam o hash antes de chamar Equals. Comprovei usando StringComparer.OrdinalIgnoreCase.GetHashCode(Ra) e verificando que est1.GetHashCode() == est1Copia.GetHashCode() resulta em true.

    3. Regra de duplicidade: Turma.MatricularEstudante impede dois registros para o mesmo Estudante usando Any com Equals. Neste projeto, a regra de duplicidade coincide com a identidade lógica do estudante (Ra).

    4. Classe associativa: Matricula foi criada porque DataMatricula, NotaFinal e Status pertencem à participação específica de um aluno em uma turma.

    5. Estados e transição proibida: Ativar() e Cancelar() alteram o estado. Ativar após cancelar foi proibido porque uma matrícula cancelada encerra o vínculo daquele aluno na turma.

    6. Associação, agregação e composição: Turma → Professor (associação simples), Turma → MaterialApoio (agregação de partes compartilháveis) e Turma → Matricula (composição com dependência de existência).

    7. Encerramento do todo: Turma.Encerrar() marca a turma como encerrada e cancela matrículas pendentes. Professor, Estudante e MaterialApoio permanecem íntegros.

    8. Responsabilidades: As validações, controle de coleção e transições de estado estão nas classes da pasta Modelo/. Program.cs apenas instancia os cenários e exibe os resultados no terminal.

    9. Erro corrigido no desenvolvimento: Inicialmente tentei usar _matriculas.Contains(estudante) (registrado no erro CS1503 acima), corrigido com Any().

    10. Melhoria para uma segunda versão: Permitir rematrícula caso a matrícula anterior esteja cancelada, diferenciando duplicidade de matrícula ativa de histórico de cancelamentos.

Experimento com HashSet<T> vs List<T>.Contains()
No Program.cs, adicionei est1 ("RA2026") a um HashSet<Estudante> e depois tentei adicionar est1Copia ("ra2026"). O HashSet.Add(est1Copia) retornou false e manteve Count = 1, pois ambos geraram o mesmo GetHashCode() e Equals() retornou true. Já em List<T>.Contains(), a busca percorre a lista chamando apenas Equals(), sem depender de GetHashCode().





