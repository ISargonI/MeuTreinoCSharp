using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using SistemaComercial.Negocio.Modelo;

namespace SistemaComercial.ConsoleApp
{
    public class ProdutoController
    {
        private List<Produto> _produtos = new();
       private readonly string CAMINHO_ARQUIVO = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "produtos.json");

        public ProdutoController()
        {
            CarregarDados();
        }

        public void IniciarMenuInterativo()
        {
            bool executando = true;
            while (executando)
            {
                Console.Clear();
                Console.WriteLine("==========================================");
                Console.WriteLine("   GERENCIAMENTO DE PRODUTOS (COM JSON)   ");
                Console.WriteLine("==========================================");
                Console.WriteLine("1 - Cadastrar Produto (Create)");
                Console.WriteLine("2 - Listar Todos (Read)");
                Console.WriteLine("3 - Consultar por ID (Read)");
                Console.WriteLine("4 - Alterar Produto (Update)");
                Console.WriteLine("5 - Remover Produto (Delete)");
                Console.WriteLine("0 - Sair");
                Console.WriteLine("==========================================");
                Console.Write("Escolha uma opção: ");
                
                string? op = Console.ReadLine();
                Console.WriteLine();

                switch (op)
                {
                    case "1":
                        CadastrarComRepeticao();
                        break;
                    case "2":
                        Listar();
                        Console.ReadLine();
                        break;
                    case "3":
                        Consultar();
                        Console.ReadLine();
                        break;
                    case "4":
                        AlterarComRepeticao();
                        break;
                    case "5":
                        Remover();
                        Console.ReadLine();
                        break;
                    case "0":
                        executando = false;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Pressione ENTER.");
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void CadastrarComRepeticao()
        {
            while (true)
            {
                try
                {
                    Console.WriteLine("--- NOVO PRODUTO (Digite 'cancelar' no nome para sair) ---");
                    Console.Write("Nome: ");
                    string nome = Console.ReadLine() ?? "";
                    if (nome.ToLower() == "cancelar") return;

                    Console.Write("Preço (R$): ");
                    decimal preco = decimal.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Estoque: ");
                    int estoque = int.Parse(Console.ReadLine() ?? "0");

                    Cadastrar(nome, preco, estoque); 
                    
                    Console.WriteLine("\n[SUCESSO] Produto cadastrado! Pressione ENTER...");
                    Console.ReadLine();
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\n[ERRO] Digite números válidos para preço e estoque.\n");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"\n[ERRO DE NEGÓCIO] {ex.Message}\n");
                }
            }
        }

        private void AlterarComRepeticao()
        {
            Console.WriteLine("--- ALTERAR PRODUTO ---");
            Console.Write("Digite o ID do produto: ");
            
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("[ERRO] ID inválido. Pressione ENTER.");
                Console.ReadLine();
                return;
            }

            var produto = _produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null)
            {
                Console.WriteLine($"[ERRO] Produto ID {id} não encontrado. Pressione ENTER.");
                Console.ReadLine();
                return;
            }

            Console.WriteLine($"\nDados Atuais: {produto}");

            while (true)
            {
                try
                {
                    Console.WriteLine("\n--- NOVOS DADOS (Digite 'cancelar' no nome para sair) ---");
                    Console.Write("Novo Nome: ");
                    string novoNome = Console.ReadLine() ?? "";
                    if (novoNome.ToLower() == "cancelar") return;

                    Console.Write("Novo Preço (R$): ");
                    decimal novoPreco = decimal.Parse(Console.ReadLine() ?? "0");

                    Console.Write("Novo Estoque: ");
                    int novoEstoque = int.Parse(Console.ReadLine() ?? "0");

                    produto.AtualizarDados(novoNome, novoPreco, novoEstoque);
                    SalvarDados();

                    Console.WriteLine("\n[SUCESSO] Produto atualizado! Pressione ENTER...");
                    Console.ReadLine();
                    break;
                }
                catch (FormatException)
                {
                    Console.WriteLine("\n[ERRO] Digite números válidos para preço e estoque.\n");
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"\n[ERRO DE NEGÓCIO] {ex.Message}\n");
                }
            }
        }

        private void Consultar()
        {
            Console.WriteLine("--- CONSULTAR PRODUTO ---");
            Console.Write("Digite o ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var produto = _produtos.FirstOrDefault(p => p.Id == id);
                if (produto != null)
                    Console.WriteLine($"\nEncontrado: {produto}");
                else
                    Console.WriteLine($"\n[ERRO] Produto ID {id} não encontrado.");
            }
            else
            {
                Console.WriteLine("[ERRO] ID inválido.");
            }
        }

        private void Remover()
        {
            Console.WriteLine("--- REMOVER PRODUTO ---");
            Console.Write("Digite o ID: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var produto = _produtos.FirstOrDefault(p => p.Id == id);
                if (produto != null)
                    {
                        Console.WriteLine($"\nProduto: {produto}");
                        Console.Write("Confirma a exclusão? (S/N): ");
                        if (Console.ReadLine()?.Trim().ToUpper() == "S")
                        {
                            _produtos.Remove(produto);
                            SalvarDados();
                            Console.WriteLine("\n[SUCESSO] Produto removido.");
                        }
                        else
                        {
                            Console.WriteLine("\nOperação cancelada.");
                        }
                    }
                else
                    Console.WriteLine($"\n[ERRO] Produto ID {id} não encontrado.");
            }
            else
            {
                Console.WriteLine("[ERRO] ID inválido.");
            }
        }

        public void Cadastrar(string nome, decimal preco, int estoque)
        {
            int proximoId = _produtos.Any() ? _produtos.Max(p => p.Id) + 1 : 1;
            var produto = new Produto(proximoId, nome, preco, estoque);
            
            _produtos.Add(produto);
            SalvarDados(); 
        }

        public void Listar()
        {
            Console.WriteLine("--- LISTA DE PRODUTOS ---");
            if (!_produtos.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado.");
                return;
            }
            foreach (var p in _produtos) Console.WriteLine(p);
        }

        private void SalvarDados()
        {
            string json = JsonSerializer.Serialize(_produtos, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(CAMINHO_ARQUIVO, json);
        }

        private void CarregarDados()
        {
            if (File.Exists(CAMINHO_ARQUIVO))
            {
                string json = File.ReadAllText(CAMINHO_ARQUIVO);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    _produtos = JsonSerializer.Deserialize<List<Produto>>(json) ?? new List<Produto>();
                }
            }
        }
    }
}