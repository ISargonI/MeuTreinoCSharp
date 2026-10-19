using System;

namespace AtendimentoUPA.Modelo
{
    public class Paciente
    {

        public string NumeroSus { get; private set; }
        public string Nome { get; private set; }
        public string QueixaPrincipal { get; private set; }

        public Paciente(string numeroSus, string nome, string queixa)
        {
            if (string.IsNullOrWhiteSpace(numeroSus))
            {
                throw new ArgumentException("O número do SUS é obrigatório.");
            }
            if (string.IsNullOrWhiteSpace(nome))
            {
                throw new ArgumentException("O nome não pode ser vazio.");
            }

            NumeroSus = numeroSus;
            Nome = nome;
            QueixaPrincipal = queixa; 
        }
    }
}