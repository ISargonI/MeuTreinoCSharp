using System;

public class ItemManutencao
{
    public Servico ServicoRealizado { get; }
    public int Quantidade { get; }
    public string? Observacao { get; } 

    public ItemManutencao(Servico servico, int quantidade, string? observacao = null)
    {
        ServicoRealizado = servico ?? throw new ArgumentNullException(nameof(servico));
        
        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.");

        Quantidade = quantidade;
        Observacao = observacao;
    }
}