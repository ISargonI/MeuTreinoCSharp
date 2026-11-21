# Guia de Distribuição e Compartilhamento do Auto Posto Oásis
**Projeto:** Auto Posto & Conveniência Oásis  
**Disciplina:** Programação Orientada a Objetos e Modelagem de Software (Prof. Dr. Alan Gavioli - UTFPR)  
**Objetivo:** Instruções práticas para compartilhar o projeto com o grupo e garantir que ele rode em qualquer computador, mesmo sem .NET instalado.

---

## 🌟 Visão Geral das Formas de Compartilhamento

Existem duas formas principais de compartilhar o trabalho:

1. **Opção 1: O Pacote Executável "Plug & Play" (Recomendado para apresentação e testes rápidos)**  
   * **Vantagem:** O colega ou professor **NÃO precisa instalar nada** (nem .NET, nem SDK, nem VS Code). É só descompactar e dar dois cliques no `.exe`.
   * **Arquivo:** Já compilado e pronto na raiz do projeto: `PostoOasis-Executavel-Windows.zip` (ou na pasta `publish/win-x64/`).

2. **Opção 2: O Código-Fonte Completo (Para o grupo programar e enviar ao professor)**  
   * **Vantagem:** Permite aos membros do grupo visualizar a arquitetura em camadas (`PostoOasis.Core`, `PostoOasis.Data`, `PostoOasis.App`), estudar os arquivos `.cs` e `.axaml`, e rodar via terminal.
   * **Requisito:** A máquina precisa do .NET 10 (ou .NET 8/9/10 SDK instalado).

---

## 🚀 Opção 1: Como Compartilhar o Executável Pronto (Qualquer PC Windows)

Nós realizamos uma publicação do tipo **Self-Contained Single-File**, que embute a máquina virtual do .NET dentro do executável.

### Onde está o arquivo?
* Na raiz do projeto:  
  📁 `c:\Users\Adriano\MeuTreinoCSharp\ProjetoModelagemPosto\PostoOasis-Executavel-Windows.zip`
* Ou a pasta descompactada:  
  📁 `c:\Users\Adriano\MeuTreinoCSharp\ProjetoModelagemPosto\publish\win-x64\`

### Como enviar para os colegas:
1. Envie o arquivo `PostoOasis-Executavel-Windows.zip` via **Google Drive, WhatsApp Web, Discord, OneDrive, E-mail ou Pendrive**.
2. O seu colega (ou o professor) faz o seguinte:
   * Clica com o botão direito no arquivo `.zip` e escolhe **"Extrair Tudo..."**.
   * Abre a pasta extraída.
   * Dá **dois cliques em `PostoOasis.App.exe`**.
3. O sistema abre imediatamente com a tela de login, todas as telas de Frentista, Caixa, RH e Gerente, e o banco de dados em memória pré-carregado.

> [!TIP]
> **Dica para o dia da apresentação na UTFPR:**  
> Copie a pasta `publish/win-x64` para um pendrive. Se o computador do laboratório ou do projetor não tiver permissão de administrador ou não tiver o .NET instalado, o aplicativo rodará perfeitamente direto do pendrive!

---

## 💻 Opção 2: Como Compartilhar o Código-Fonte Limpo

Se os membros do grupo querem abrir o projeto no VS Code ou Visual Studio para estudar ou alterar código:

### 1. Limpando os arquivos temporários antes de compactar
As pastas `bin` e `obj` guardam arquivos temporários de compilação que pesam centenas de megabytes e não devem ser enviados por e-mail ou Git.

Para gerar um zip leve contendo apenas o código-fonte, a documentação e os assets:

```powershell
# No PowerShell, na raiz do projeto:
dotnet clean
```

As pastas essenciais que devem ir no zip do código-fonte são:
- 📁 `src/` (todos os 3 projetos: Core, Data e App)
- 📁 `docs/` (toda a documentação dos requisitos e arquitetura)
- 📄 `AutoPostoOasis.slnx` (arquivo da solução)
- 📄 `nuget.config`

### 2. Como o colega abre e executa o código-fonte:
1. O colega extrai a pasta do projeto.
2. Abre a pasta no **VS Code** (`Arquivo -> Abrir Pasta`).
3. Abre o Terminal do VS Code (`Ctrl + '`) e digita:
   ```powershell
   dotnet run --project src/PostoOasis.App/PostoOasis.App.csproj
   ```
4. O .NET compilará os 3 projetos automaticamente e abrirá a interface gráfica.

---

## 🌐 Opção 3: Usando o GitHub (Ideal para Trabalho em Grupo)

Se o grupo quiser usar controle de versão colaborativo:

1. Na raiz do projeto, inicialize o Git (caso ainda não tenha):
   ```bash
   git init
   dotnet new gitignore
   git add .
   git commit -m "feat: Sistema Auto Posto Oasis com Avalonia UI MVC e RF01-RF20"
   ```
2. Crie um repositório no GitHub (público ou privado) e vincule:
   ```bash
   git remote add origin https://github.com/SEU_USUARIO/AutoPostoOasis.git
   git branch -M main
   git push -u origin main
   ```
3. Os outros membros só precisam rodar:
   ```bash
   git clone https://github.com/SEU_USUARIO/AutoPostoOasis.git
   cd AutoPostoOasis
   dotnet run --project src/PostoOasis.App/PostoOasis.App.csproj
   ```

---

## 🐧 E se algum colega ou professor usar Linux ou macOS?

O **Avalonia UI** é 100% multiplataforma. Se alguém no grupo usa Ubuntu, Fedora ou Mac:

* **No Linux:**  
  Basta ter o .NET instalado e rodar o mesmo comando:
  ```bash
  dotnet run --project src/PostoOasis.App/PostoOasis.App.csproj
  ```
* **Para gerar um executável próprio de Linux:**
  ```bash
  dotnet publish src/PostoOasis.App/PostoOasis.App.csproj -c Release -r linux-x64 --self-contained true -o publish/linux-x64
  ```
* **Para gerar um executável próprio de macOS:**
  ```bash
  dotnet publish src/PostoOasis.App/PostoOasis.App.csproj -c Release -r osx-x64 --self-contained true -o publish/osx-x64
  ```

---

## 📋 Resumo dos Arquivos Prontos na sua Máquina

| Arquivo / Pasta | Para quem serve | Como usar |
| :--- | :--- | :--- |
| `PostoOasis-Executavel-Windows.zip` | Colegas sem .NET / Apresentação | Descompactar e clicar em `PostoOasis.App.exe` |
| `publish/win-x64/` | Pasta pronta para pendrive | Copiar para pendrive e rodar direto em qualquer PC |
| `src/` + `AutoPostoOasis.slnx` | Membros do grupo programadores | Abrir no VS Code e rodar `dotnet run` |
| `docs/` | Estudo dos requisitos e prints | Guias de estudo e entrega do professor |
