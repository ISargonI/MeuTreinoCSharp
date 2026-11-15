using System;
using System.Collections.Generic;
using SistemaGerenciadoresVeiculos.Modelo;

namespace SistemaGerenciadoresVeiculos
{
    class Program
    {
        static void Main(string[] args)
        {
            Veiculo veiculo = new Veiculo("ABC1234", "Toyota", "Corolla", 2021, 50000);
            veiculo.ExibirDados();

            veiculo.RegistrarQuilometragem(1500);
            veiculo.ExibirDados();

            Caminhao caminhao = new Caminhao("XYZ7890", "Volvo", "FH16", 2020, 100000, 25000, 5);
            caminhao.ExibirDados();
        }
    }
}


