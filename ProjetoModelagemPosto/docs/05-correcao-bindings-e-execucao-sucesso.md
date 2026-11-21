# Auto Posto Oásis - Resolução de Erro de Inicialização XAML no Avalonia UI

> **Disciplina:** Programação Orientada a Objetos e Modelagem de Software  
> **Instituição:** UTFPR - Câmpus Medianeira  
> **Docente:** Prof. Dr. Alan Gavioli  
> **Estudante:** Adriano Rodrigues Alves  

---

## 1. O que estava acontecendo?

Quando você executou `dotnet run`, o programa fechava silenciosamente sem abrir a janela.

Ao investigar o ponto de entrada da aplicação (`Program.cs`) e capturar o log de erro, identificamos que o parser XAML do Avalonia lançou a seguinte exceção:

```text
System.ArgumentException: Unable to resolve type vm:MainWindowViewModel
   at Avalonia.Markup.Xaml.XamlIl.Runtime.XamlIlRuntimeHelpers.XamlTypeResolver.Resolve
   in MainWindow.axaml:line 215
```

### Causa raiz:
Dentro dos `DataTemplate` (os blocos que repetem os cartões de cada Bomba, Produto e Venda), estávamos utilizando uma conversão explícita de tipo no XAML:
```xml
<!-- Forma incorreta que gerava falha de resolução em tempo de execução -->
Command="{Binding $parent[Window].((vm:MainWindowViewModel)DataContext).IniciarAbastecimentoLivreCommand}"
```

No Avalonia UI (consultado na documentação oficial via Context7), os templates resolvem comandos do DataContext ancestral automaticamente sem necessidade de conversão manual:
```xml
<!-- Forma correta e canônica do Avalonia UI -->
Command="{Binding $parent[Window].DataContext.IniciarAbastecimentoLivreCommand}"
```

---

## 2. Ajustes Realizados:

1. **Correção dos Bindings no `MainWindow.axaml`:** Substituímos as 9 ocorrências de conversão pelo binding direto ao `DataContext` ancestral.
2. **Tratamento de Exceções no `Program.cs`:** Adicionamos bloco `try/catch` para que, caso ocorra qualquer imprevisto futuro, o erro seja impresso no terminal em vez de fechar silenciosamente.
3. **Teste de Execução:** A aplicação foi iniciada e testada, mantendo o processo ativo e estável sem nenhuma exceção.

---

## 3. Como Rodar Agora no seu Terminal

Execute novamente no seu terminal:

```powershell
dotnet run --project src/PostoOasis.App/PostoOasis.App.csproj
```

A janela do **Auto Posto Oásis** abrirá diretamente na sua tela!
