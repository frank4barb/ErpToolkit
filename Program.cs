using ErpToolkit.Extensions;
using ErpToolkit.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ErpToolkit
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                var isDebugging = !(Debugger.IsAttached || args.Contains("--console"));
                ErpContext.Init(); // Init Erp Model before start services
                var hostBuilder = new HostBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        services.AddHostedService<ServiceScheduler>();
                        services.AddHostedService<ServiceListener>();
                    });
                if (isDebugging)
                {
                    await hostBuilder.RunTheServiceAsync();
                }
                else
                {
                    await hostBuilder.RunConsoleAsync();
                }
            }
            catch (Exception ex)
            {
                // ERRORE ALL'AVVIO: Mostra il messaggio di errore nella console
                Console.WriteLine($"Errore: {ex.Message}");

                // Esci dal programma
                Environment.Exit(1);
            }
        }
    }
}
