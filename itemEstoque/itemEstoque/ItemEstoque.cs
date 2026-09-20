using System;

public class ItemEstoque
{
    // Campos privados protegem o estado interno do objeto.
    // O tipo string? indica que o campo pode, inicialmente, ser nulo[cite: 3, 4].
    private string? _nome;
    private int _quantidade;

    // Propriedades públicas usadas apenas para CONSULTAR os dados, sem 'set'[cite: 3].
    public string? Nome => _nome;
    public int Quantidade => _quantidade;

    // Método responsável por alterar o nome, substituindo o 'set' público[cite: 3].
    public void AtualizarNome(string? nome)
    {
        // string.IsNullOrWhiteSpace valida se é nulo, vazio ou apenas espaços[cite: 3, 4].
        if (string.IsNullOrWhiteSpace(nome))
        {
            throw new ArgumentException("O nome do item não pode ser nulo, vazio ou conter apenas espaços.", nameof(nome));
        }
        
        _nome = nome;
    }

    // Método responsável por alterar a quantidade[cite: 3].
    public void AtualizarQuantidade(int quantidade)
    {
        if (quantidade < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantidade), quantidade, "A quantidade não pode ser negativa.");
        }
        
        _quantidade = quantidade;
    }
}