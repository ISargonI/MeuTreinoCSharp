using System;
using System.Collections.Generic;
using System.Linq;

public class OrdemServico
{
    public string Numero { get; }

    // Coleção encapsulada
    private readonly List<ItemManutencao> _itens;

    // Visão de leitura para o exterior
    public IReadOnlyCollection<ItemManutencao> Itens => _itens.AsReadOnly();

    public OrdemServico(string numero)
    {
        if (string.IsNullOrWhiteSpace(numero))
            throw new ArgumentException("O número da ordem é obrigatório.");

        Numero = numero;
        _itens = new List<ItemManutencao>(); // Nasce sem itens
    }

    public void AdicionarItem(ItemManutencao item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

      
        if (_itens.Any(i => i.ServicoRealizado.Equals(item.ServicoRealizado)))
        {
            throw new InvalidOperationException($"O serviço '{item.ServicoRealizado.Descricao}' já existe nesta ordem de serviço.");
        }

        _itens.Add(item);
    }

    public void RemoverItemPorServico(Servico servico)
    {
        if (servico == null)
            throw new ArgumentNullException(nameof(servico));

        var itemARemover = _itens.FirstOrDefault(i => i.ServicoRealizado.Equals(servico));
        
        if (itemARemover == null)
            throw new InvalidOperationException($"O serviço '{servico.Descricao}' não consta nos itens desta ordem.");

        _itens.Remove(itemARemover);
    }
}