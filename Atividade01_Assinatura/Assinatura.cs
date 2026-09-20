using System;

/*

public class Assinatura   //versão inicial sem validação
{
    
    public string? Plano { get; set; }
    public decimal ValorMensal { get; set; }
    public int LimiteUsuarios { get; set; }
}

*/

public class Assinatura
{
    public string Plano 
    { 
        get; 
        private set 
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("O nome do plano é obrigatório e não pode ser vazio.", nameof(value));
            }
            field = value;
        } 
    }

    public decimal ValorMensal 
    { 
        get; 
        private set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "O valor mensal deve ser maior que zero.");
            }
            field = value;
        }
    }

    public int LimiteUsuarios 
    { 
        get; 
        private set
        {
            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "O limite de usuários deve ser maior que zero.");
            }
            field = value;
        }
    }

    
    public Assinatura(string? plano, decimal valorMensal, int limiteUsuarios)
    {
    
        Plano = plano!; 
        ValorMensal = valorMensal;
        LimiteUsuarios = limiteUsuarios;
    }

    public void AlterarPlano(string? plano)
    {
        Plano = plano!; 
    }

    public void AlterarValorMensal(decimal valor)
    {
        ValorMensal = valor; 
    }

    public void AlterarLimiteUsuarios(int limite)
    {
        LimiteUsuarios = limite; 
    }
}