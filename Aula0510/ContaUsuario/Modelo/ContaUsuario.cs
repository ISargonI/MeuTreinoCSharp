using System;
using System.Collections.Generic;

public class ContaUsuario
{
    public string Email { get; }

    private readonly List<Dispositivo> _dispositivosAutorizados;

    
    public IReadOnlyCollection<Dispositivo> DispositivosAutorizados => _dispositivosAutorizados.AsReadOnly();

    public ContaUsuario(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("O e-mail da conta é obrigatório.");

        Email = email;
        _dispositivosAutorizados = new List<Dispositivo>();
    }

    public void AutorizarDispositivo(Dispositivo dispositivo)
    {
        if (dispositivo == null)
            throw new ArgumentNullException(nameof(dispositivo), "O dispositivo não pode ser nulo.");

        if (_dispositivosAutorizados.Count >= 5)
            throw new InvalidOperationException("A conta atingiu o limite máximo de 5 dispositivos autorizados.");

        if (_dispositivosAutorizados.Contains(dispositivo))
            throw new InvalidOperationException($"O dispositivo com identificador '{dispositivo.Identificador}' já está autorizado.");

        _dispositivosAutorizados.Add(dispositivo);
    }

    public void RevogarDispositivo(Dispositivo dispositivo)
    {
        if (dispositivo == null)
            throw new ArgumentNullException(nameof(dispositivo));

        if (!_dispositivosAutorizados.Contains(dispositivo))
            throw new InvalidOperationException($"O dispositivo '{dispositivo.Nome}' não está autorizado nesta conta.");

        _dispositivosAutorizados.Remove(dispositivo);
    }
}