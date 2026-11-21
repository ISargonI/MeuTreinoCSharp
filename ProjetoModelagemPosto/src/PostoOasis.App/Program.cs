using System;
using System.IO;
using Avalonia;

namespace PostoOasis.App;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            File.WriteAllText("crash.log", ex.ToString());
            Console.Error.WriteLine("ERRO FATAL AO INICIAR AVALONIA:");
            Console.Error.WriteLine(ex.ToString());
            throw;
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
