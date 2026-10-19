using System;
using System.Collections.Generic;

namespace Biblioteca.Modelo
{
    public class Leitor
    {
        public string Nome { get; private set; }

        private const int LimiteEmprestimos = 3;

        private readonly List<Livro> _emprestimos = new List<Livro>();

        public IReadOnlyCollection<Livro> Emprestimos => _emprestimos.AsReadOnly();

        public Leitor(string nome)
        {
            Nome = nome;
        }

        public void RequisitarLivro(Livro livro)
        {
  
            if (livro == null)
            {
                throw new ArgumentNullException(nameof(livro), "O livro não pode ser nulo.");
            }

            // 2. Respeitar capacidade máxima
            if (_emprestimos.Count >= LimiteEmprestimos)
            {
                throw new InvalidOperationException($"O leitor {Nome} já atingiu o limite de {LimiteEmprestimos} empréstimos simultâneos.");
            }

            foreach (var emp in _emprestimos)
            {
                if (emp.ISBN == livro.ISBN)
                {
                    throw new InvalidOperationException($"O livro '{livro.Titulo}' já está na posse deste leitor.");
                }
            }

            _emprestimos.Add(livro);
        }

        public void DevolverLivro(Livro livro)
        {
            if (livro == null)
            {
                throw new ArgumentNullException(nameof(livro));
            }

            if (!_emprestimos.Contains(livro))
            {
                throw new InvalidOperationException($"O livro '{livro.Titulo}' não consta nos empréstimos do leitor {Nome}.");
            }

            _emprestimos.Remove(livro);
        }
    }
}