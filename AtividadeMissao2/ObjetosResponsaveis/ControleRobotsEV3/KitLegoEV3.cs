using System;

public class KitLegoEV3
{
  
    public string Identificacao { get; }
    
   
    public int NivelBateria { get; private set; }
    public bool EmUso { get; private set; }

    public KitLegoEV3(string identificacao)
    {
        if (string.IsNullOrWhiteSpace(identificacao))
            throw new ArgumentException("A identificação do kit Lego não pode ser vazia.");

        Identificacao = identificacao;
        NivelBateria = 100; 
        EmUso = false;     
    }

  
    public void EmprestarParaMinicurso()
    {
        if (EmUso)
            throw new InvalidOperationException("Este kit EV3 já está em uso na oficina.");
        if (NivelBateria < 20)
            throw new InvalidOperationException("Bateria muito baixa (< 20%). Recarregue o kit antes do minicurso.");

        EmUso = true;
    }


    public void Devolver(int bateriaRestante)
    {
        if (!EmUso)
            throw new InvalidOperationException("O kit já consta como devolvido no laboratório.");
        
    
        if (bateriaRestante < 0 || bateriaRestante > 100)
            throw new ArgumentOutOfRangeException(nameof(bateriaRestante), "O nível de bateria deve estar entre 0 e 100.");

        NivelBateria = bateriaRestante;
        EmUso = false;
    }
}