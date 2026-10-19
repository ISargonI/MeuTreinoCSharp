using System;
using System.Collections.Generic;

namespace SalasDeCinema.Modelo
{
    public class Sessao
    {
        public string Filme { get; private set; }
        public int CapacidadeMaxima { get; private set; }
        
        private readonly List<Ingresso> _ingressos = new List<Ingresso>();

        public IReadOnlyCollection<Ingresso> Ingressos => _ingressos.AsReadOnly();

        public Sessao(string filme, int capacidadeMaxima)
        {
            Filme = filme;
            CapacidadeMaxima = capacidadeMaxima;
        }

        public void VenderIngresso(Ingresso ingresso)
        {
            if (ingresso == null)
            {
                throw new ArgumentNullException(nameof(ingresso), "Ingresso inválido (nulo).");
            }

            if (_ingressos.Count >= CapacidadeMaxima)
            {
                throw new InvalidOperationException($"Sessão esgotada. Capacidade máxima é {CapacidadeMaxima}.");
            }

            foreach (var ing in _ingressos)
            {
                if (ing.Assento == ingresso.Assento)
                {
                    throw new InvalidOperationException($"O assento {ingresso.Assento} já está ocupado nesta sessão.");
                }
            }

            _ingressos.Add(ingresso);
        }

        public void CancelarIngresso(Ingresso ingresso)
        {
            if (ingresso == null)
            {
                throw new ArgumentNullException(nameof(ingresso));
            }

            if (!_ingressos.Contains(ingresso))
            {
                throw new InvalidOperationException("Este ingresso não pertence a esta sessão.");
            }

            _ingressos.Remove(ingresso);
        }
    }
}