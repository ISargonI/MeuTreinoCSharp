using System;

namespace SalasDeCinema.Modelo
{
    public class Ingresso
    {
        public string Assento { get; private set; }
        public string Tipo { get; private set; } // Ex: "Inteira", "Meia"

        public Ingresso(string assento, string tipo)
        {
            if (string.IsNullOrWhiteSpace(assento))
            {
                throw new ArgumentException("O assento não pode ser vazio.");
            }

            Assento = assento;
            Tipo = tipo;
        }
    }
}