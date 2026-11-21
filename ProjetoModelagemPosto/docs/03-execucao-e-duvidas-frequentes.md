# Auto Posto Oásis - Execução e Dúvidas Frequentes

> **Disciplina:** Programação Orientada a Objetos e Modelagem de Software  
> **Instituição:** UTFPR - Câmpus Medianeira  
> **Docente:** Prof. Dr. Alan Gavioli  
> **Estudante:** Adriano Rodrigues Alves  

---

## 1. Preciso instalar os "Avalonia Templates" para rodar o projeto?

**Resposta curta: NÃO!** Você pode executar o projeto imediatamente com `dotnet run`.

### Entendendo a diferença conceitual:

* **O que é o `Avalonia.Templates`?**  
  Ele serve **única e exclusivamente** para o comando `dotnet new`. Ou seja, é apenas a "fôrma de bolo" usada quando você quer começar um projeto do zero absoluto pelo terminal (ex: `dotnet new avalonia.mvvm`).
  
* **O que é o projeto já existente?**  
  Como os arquivos `.csproj`, os códigos C# (`MainWindowViewModel.cs`), as telas XAML (`MainWindow.axaml`) e as dependências NuGet já foram criados e compilados com sucesso no seu computador, **o projeto já existe por completo**.

Portanto, **você não precisa esperar nem instalar nenhum template** para compilar ou rodar o software.

---

## 2. Como Executar o Sistema Agora

Basta abrir o terminal na pasta do projeto e rodar:

```powershell
dotnet run --project src/PostoOasis.App/PostoOasis.App.csproj
```

O .NET irá iniciar a interface gráfica do **Auto Posto Oásis**, e a janela com o menu lateral e todas as regras de negócio aparecerá na sua tela.
