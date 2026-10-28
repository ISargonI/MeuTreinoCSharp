using System;

namespace SistemaComercial.Negocio.Modelo
{
    public class Produto
    {
        public int Id { get; private set; }
        public string Nome { get; private set; } = string.Empty;
        public decimal Preco { get; private set; }
        public int Estoque { get; private set; }

        public Produto(int id, string nome, decimal preco, int estoque)
        {
            if (id <= 0)
                throw new ArgumentException("O ID do produto deve ser um número maior que zero.");

            Id = id;
            DefinirNome(nome);
            DefinirPreco(preco);
            DefinirEstoque(estoque);
        }

        public void AtualizarDados(string novoNome, decimal novoPreco, int novoEstoque)
        {
            DefinirNome(novoNome);
            DefinirPreco(novoPreco);
            DefinirEstoque(novoEstoque);
        }

        private void DefinirNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do produto é obrigatório e não pode estar vazio.");

            if (nome.Trim().Length < 3)
                throw new ArgumentException("O nome do produto deve ter pelo menos 3 caracteres.");

            Nome = nome.Trim();
        }

        private void DefinirPreco(decimal preco)
        {
            if (preco <= 0)
                throw new ArgumentException("O preço do produto deve ser maior que zero.");

            Preco = preco;
        }

        private void DefinirEstoque(int estoque)
        {
            if (estoque < 0)
                throw new ArgumentException("A quantidade em estoque não pode ser negativa.");

            Estoque = estoque;
        }

        public override string ToString()
        {
            return $"[ID: {Id}] Nome: {Nome} | Preço: {Preco:C} | Estoque: {Estoque} un.";
        }
    }
}