
using Side2D.Server.Infrastructure;

namespace Side2D.Server;

public class Program
{
    private static ServerManager? _serverManager;
    public static async Task Main(string[] args)
    {
        _serverManager = new ServerManager();
        _serverManager.Start();
        
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            Console.WriteLine("Finalizando servidor...");
            _serverManager?.Stop();
            
            _serverManager?.Start();
            
            //_serverManager?.Dispose();
            eventArgs.Cancel = true; // Cancela a finalização automática
            
            // Finaliza o processo
            //Environment.Exit(0);
        };

        Console.WriteLine("Servidor iniciado. Pressione Ctrl+C para encerrar.");

        // Mantém o servidor ativo até que o processo seja interrompido
        await Task.Delay(Timeout.Infinite);
    }
}