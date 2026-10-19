using System;
using System.Collections.Generic;

namespace AtendimentoUPA.Modelo
{
    public class Medico
    {
        public string Nome { get; private set; }
        public string Especialidade { get; private set; }
        

        private const int LimiteFila = 3;

        private readonly List<Paciente> _filaEspera = new List<Paciente>();

        public IReadOnlyCollection<Paciente> FilaEspera => _filaEspera.AsReadOnly();

        public Medico(string nome, string especialidade)
        {
            Nome = nome;
            Especialidade = especialidade;
        }

        public void AgendarPaciente(Paciente paciente)
        {

            if (paciente == null)
            {
                throw new ArgumentNullException(nameof(paciente), "Paciente não pode ser nulo.");
            }

            if (_filaEspera.Count >= LimiteFila)
            {
                throw new InvalidOperationException($"A fila do Dr(a). {Nome} está cheia (limite de {LimiteFila}).");
            }

            foreach (var p in _filaEspera)
            {
                if (p.NumeroSus == paciente.NumeroSus)
                {
                    throw new InvalidOperationException($"O paciente {paciente.Nome} já está na fila de espera.");
                }
            }

            _filaEspera.Add(paciente);
        }

        public void AtenderPaciente(Paciente paciente)
        {
            if (paciente == null)
            {
                throw new ArgumentNullException(nameof(paciente));
            }

            if (!_filaEspera.Contains(paciente))
            {
                throw new InvalidOperationException("Este paciente não está na fila de espera deste médico.");
            }

            _filaEspera.Remove(paciente);
        } 
    }
}