using System;

namespace Biblioteca.Modelo
{
    public class Livro
    {
  
        public string ISBN { get; private set; }
        public string Titulo { get; private set; }

        public Livro(string isbn, string titulo)
        {
            if (string.IsNullOrWhiteSpace(isbn))
            {
                throw new ArgumentException("O ISBN não pode ser vazio.");
            }
            if (string.IsNullOrWhiteSpace(titulo))
            {
                throw new ArgumentException("O título não pode ser vazio.");
            }

            ISBN = isbn;
            Titulo = titulo;
        }
    }
}