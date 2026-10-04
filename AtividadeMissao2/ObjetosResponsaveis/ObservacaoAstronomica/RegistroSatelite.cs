using System;

public class RegistroSatelite
{
    public string Nome { get; }
    
    
    public double Azimute { get; private set; }
    public int MinutosObservados { get; private set; }
    public bool RastreioAtivo { get; private set; }

    
    public RegistroSatelite(string nome, double azimuteInicial)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do satélite não pode ser vazio.");
        if (azimuteInicial < 0 || azimuteInicial > 360)
            throw new ArgumentOutOfRangeException(nameof(azimuteInicial), "O azimute deve estar entre 0 e 360 graus.");

        Nome = nome;
        Azimute = azimuteInicial;
        MinutosObservados = 0;
        RastreioAtivo = true;
    }

    public void AdicionarTempoVisibilidade(int minutos)
    {
        if (!RastreioAtivo)
            throw new InvalidOperationException("O rastreio deste satélite já foi encerrado e não aceita mais tempo.");
        if (minutos <= 0)
            throw new ArgumentException("O tempo adicionado deve ser maior que zero.");

        MinutosObservados += minutos;
    }

  
    public void EncerrarRastreio()
    {
        if (!RastreioAtivo)
            throw new InvalidOperationException("O rastreio já está encerrado.");

        RastreioAtivo = false;
    }
}