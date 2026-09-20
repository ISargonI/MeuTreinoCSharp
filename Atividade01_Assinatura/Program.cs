using System;


/*
using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== CRIANDO UMA ASSINATURA FRÁGIL ===");
        
        Assinatura assinatura = new Assinatura();

        assinatura.Plano = "   ";       // Erro de domínio: só tem espaços!
        assinatura.ValorMensal = -50m;   // Erro de domínio: valor negativo!
        assinatura.LimiteUsuarios = 0;   // Erro de domínio: limite zero!

        Console.WriteLine("Assinatura gravada com sucesso (*porém com estado corrompido):");
        Console.WriteLine($"Plano: '{assinatura.Plano}'");
        Console.WriteLine($"Valor: R$ {assinatura.ValorMensal}");
        Console.WriteLine($"Usuários: {assinatura.LimiteUsuarios}");
        
        // O compilador aceita isso perfeitamente, provando que código
        // que compila não é o mesmo que um objeto válido.
    }
}
*/
class Program
{
    static void Main()
    {
        Console.WriteLine("=== 1. CRIAÇÃO VÁLIDA ===");
        Assinatura assinatura = new Assinatura("Plano Premium", 49.90m, 5);
        Console.WriteLine($"Criado: {assinatura.Plano} | Valor: R$ {assinatura.ValorMensal} | Usuários: {assinatura.LimiteUsuarios}\n");

        Console.WriteLine("=== 2. TENTATIVAS DE CRIAÇÃO INVÁLIDA ===");
        
        try { new Assinatura("", 49.90m, 5); }
        catch (Exception ex) { Console.WriteLine($"Erro Plano: {ex.Message}"); }

        try { new Assinatura("Básico", 0m, 5); }
        catch (Exception ex) { Console.WriteLine($"Erro Valor: {ex.Message}"); }

        try { new Assinatura("Básico", 49.90m, -2); }
        catch (Exception ex) { Console.WriteLine($"Erro Usuários: {ex.Message}\n"); }

        Console.WriteLine("=== 3. ALTERAÇÕES VÁLIDAS ===");
        assinatura.AlterarPlano("Plano Ultra");
        assinatura.AlterarValorMensal(89.90m);
        assinatura.AlterarLimiteUsuarios(10);
        Console.WriteLine($"Atualizado: {assinatura.Plano} | Valor: R$ {assinatura.ValorMensal} | Usuários: {assinatura.LimiteUsuarios}\n");

        Console.WriteLine("=== 4. TENTATIVAS DE ALTERAÇÃO INVÁLIDA ===");
        
        try { assinatura.AlterarPlano(null); }
        catch (Exception ex) { Console.WriteLine($"Erro ao alterar plano: {ex.Message}"); }

        try { assinatura.AlterarValorMensal(-10m); }
        catch (Exception ex) { Console.WriteLine($"Erro ao alterar valor: {ex.Message}"); }

        try { assinatura.AlterarLimiteUsuarios(0); }
        catch (Exception ex) { Console.WriteLine($"Erro ao alterar limite: {ex.Message}"); }
    }
}
