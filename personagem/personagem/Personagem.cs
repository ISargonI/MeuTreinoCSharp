public class Personagem
{
    public string? Nome 
    { 
        get; 
        set 
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                field = value;
            }
        } 
    }

    public int Nivel 
    { 
        get; 
        set 
        {
            if (value >= 1 && value <= 100)
            {
                field = value;
            }
        } 
    }

    public int Energia 
    { 
        get; 
        set 
        {
            if (value >= 0 && value <= 100)
            {
                field = value;
            }
        } 
    }

    public double Velocidade 
    { 
        get; 
        set 
        {
            if (value > 0 && value <= 50)
            {
                field = value;
            }
        } 
    }
}