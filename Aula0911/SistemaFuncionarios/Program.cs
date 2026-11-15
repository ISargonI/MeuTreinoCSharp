using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        var funcionarios = new List<Funcionario>
        {
            new FuncionarioAdministrativo("Alice", "001", 5000.0, "RH", "Analista"),
            new Vendedor("Bob", "002", 3000.0, 10.0, 10000.0)
        };

        foreach (var f in funcionarios)
        {
            f.ExibirDados();
        }
    }
}
