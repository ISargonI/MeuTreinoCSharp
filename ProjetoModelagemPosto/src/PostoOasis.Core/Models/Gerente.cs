namespace PostoOasis.Core.Models;

// Conceito POO: Herança - Gerente herda de Funcionario e adiciona permissões especiais
public class Gerente : Funcionario
{
    public string PermissoesGerenciais { get; set; } = "TOTAL";

    public Gerente()
    {
        Cargo = "Gerente";
        Salario = 4500.00m;
    }

    // RN06: Apenas perfil RH ou Gerente pode alterar salários
    public bool PodeAlterarSalario() => true;

    // RN16: Fechamento de caixa só validado por Gerente
    public bool PodeValidarFechamentoCaixa() => true;

    // RN19: Ajuste manual de estoque exige aprovação de Gerente
    public bool PodeAutorizarAjusteEstoque() => true;
}
