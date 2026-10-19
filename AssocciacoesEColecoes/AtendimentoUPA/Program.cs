using System;
using AtendimentoUPA.Modelo;

namespace AtendimentoUPA
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("--- GESTÃO DE ATENDIMENTO UPA ---");
            
            Medico medico = new Medico("Ana Silva", "Clínica Geral");

            Paciente p1 = new Paciente("111222", "Carlos Gomes", "Febre alta");
            Paciente p2 = new Paciente("333444", "Beatriz Lima", "Dor de cabeça");
            Paciente pDuplicado = new Paciente("111222", "Carlos Gomes", "Febre e tosse"); 
            Paciente p3 = new Paciente("555666", "João Costa", "Corte no braço");
            Paciente p4 = new Paciente("777888", "Diana Dias", "Tonturas"); 
 
            try
            {
                Console.WriteLine($"\nAdicionando pacientes à fila da Dra. {medico.Nome}...");
                medico.AgendarPaciente(p1);
                medico.AgendarPaciente(p2);
                Console.WriteLine("Pacientes agendados com sucesso.");

                Console.WriteLine("\nTentando adicionar um paciente com o mesmo Número de Cadastro SUS...");
                medico.AgendarPaciente(pDuplicado);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO REJEITADO: {ex.Message}");
            }

            try
            {

                medico.AgendarPaciente(p3);

                Console.WriteLine("\nTentando adicionar um 4º paciente à fila...");
                medico.AgendarPaciente(p4);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERRO REJEITADO: {ex.Message}");
            }

            Console.WriteLine("\n--- FILA DE ESPERA ATUAL ---");
            foreach (var paciente in medico.FilaEspera)
            {
                Console.WriteLine($"- Cadastro SUS: {paciente.NumeroSus} | {paciente.Nome} | Queixa: {paciente.QueixaPrincipal}");
            }

            Console.WriteLine($"\nA atender a paciente '{p2.Nome}'...");
            medico.AtenderPaciente(p2);

            Console.WriteLine("\n--- FILA APÓS O ATENDIMENTO ---");
            foreach (var paciente in medico.FilaEspera)
            {
                Console.WriteLine($"- Cadastro SUS: {paciente.NumeroSus} | {paciente.Nome}"); 
            }
        }
    }
}