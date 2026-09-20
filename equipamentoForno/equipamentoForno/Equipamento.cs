public class Equipamento
{
    public string? Descricao 
    { 
        get; 
        set 
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                field = value; // Grava no campo oculto gerado pelo compilador
            }
        } 
    }

    public double Temperatura 
    { 
        get; 
        set 
        {
            if (value >= 10 && value <= 80)
            {
                field = value;
            }
        } 
    }
}