

namespace TestApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var server = new Server.Services.Initializer.ServerManager();
        
        server.Start();
        
        Console.CancelKeyPress += (sender, eventArgs) =>
        {
            Console.WriteLine("Finalizando servidor...");
            server?.Stop();
            
            server?.Dispose();
            
            //_serverManager?.Dispose();
            eventArgs.Cancel = true; // Cancela a finalização automática
            
            // Alterar o Task.Delay para um valor menor para finalizar o processo mais rapidamente
            Environment.Exit(0);
        };

        Console.WriteLine("Servidor iniciado. Pressione Ctrl+C para encerrar.");

        // Mantém o servidor ativo até que o processo seja interrompido
        await Task.Delay(Timeout.Infinite);
    }
}