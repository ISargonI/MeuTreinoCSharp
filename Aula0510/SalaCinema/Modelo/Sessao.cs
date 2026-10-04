using System;
using System.Collections.Generic;

public class Sessao
{
    public string Filme { get; }
    public DateTime Horario { get; }
    public int CapacidadeMaxima { get; }

    
    private readonly List<Assento> _assentosReservados;

    public IReadOnlyCollection<Assento> AssentosReservados => _assentosReservados.AsReadOnly();

    public Sessao(string filme, DateTime horario, int capacidadeMaxima)
    {
        if (string.IsNullOrWhiteSpace(filme))
            throw new ArgumentException("O filme é obrigatório para a sessão.");
        if (capacidadeMaxima <= 0)
            throw new ArgumentException("A capacidade máxima deve ser maior que zero.");

        Filme = filme;
        Horario = horario;
        CapacidadeMaxima = capacidadeMaxima;
        
     
        _assentosReservados = new List<Assento>();
    }

    public void ReservarAssento(Assento assento)
    {
        if (assento == null)
            throw new ArgumentNullException(nameof(assento), "O assento não pode ser nulo.");

        if (_assentosReservados.Count >= CapacidadeMaxima)
            throw new InvalidOperationException("A capacidade máxima de reservas para esta sessão já foi atingida.");

        if (_assentosReservados.Contains(assento))
            throw new InvalidOperationException($"O assento '{assento.Codigo}' já está reservado nesta sessão.");

        _assentosReservados.Add(assento);
    }

    public void CancelarReserva(Assento assento)
    {
        if (assento == null)
            throw new ArgumentNullException(nameof(assento));

        if (!_assentosReservados.Contains(assento))
            throw new InvalidOperationException($"A reserva para o assento '{assento.Codigo}' não foi encontrada nesta sessão.");

        _assentosReservados.Remove(assento);
    }
}