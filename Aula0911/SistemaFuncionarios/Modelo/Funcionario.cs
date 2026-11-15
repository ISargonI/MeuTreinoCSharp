using System;
using System.Collections.Generic;

public class Funcionario
{
    public string Nome { get; private set; }
    public string Matricula { get; private set; }
    public double SalarioBase { get; private set; }

    public Funcionario(string nome, string matricula, double salarioBase)
    {
        if (string.IsNullOrWhiteSpace(nome)) throw new ArgumentException("Nome inválido.");
        if (string.IsNullOrWhiteSpace(matricula)) throw new ArgumentException("Matrícula inválida.");
        if (salarioBase <= 0) throw new ArgumentOutOfRangeException("Salário deve ser maior que zero.");

        Nome = nome;
        Matricula = matricula;
        SalarioBase = salarioBase;
    }

    public virtual void ExibirDados()
    {
        Console.WriteLine($"Nome: {Nome} | Matrícula: {Matricula} | Salário Base: R$ {SalarioBase:F2}");
    }
}