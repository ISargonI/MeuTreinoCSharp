using System;
using System.Collections.Generic;
public class FuncionarioAdministrativo : Funcionario
{
    public string SetorAtuacao { get; private set; }
    public string CargoAdministrativo { get; private set; }

    public FuncionarioAdministrativo(string nome, string matricula, double salarioBase, string setor, string cargo) 
        : base(nome, matricula, salarioBase) // Inicializa a base
    {
        if (string.IsNullOrWhiteSpace(setor)) throw new ArgumentException("Setor inválido.");
        if (string.IsNullOrWhiteSpace(cargo)) throw new ArgumentException("Cargo inválido.");
        
        SetorAtuacao = setor;
        CargoAdministrativo = cargo;
    }

    public override void ExibirDados()
    {
        base.ExibirDados();
        Console.WriteLine($"Setor: {SetorAtuacao} | Cargo: {CargoAdministrativo}");
    }
}