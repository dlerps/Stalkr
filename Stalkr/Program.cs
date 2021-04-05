using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Stalkr.Core.Logging;

namespace Stalkr
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = LoggrFactory.CreateGlobalLogger();

            try
            {
                await ConfigHost().RunAsync();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "Stalkr terminated with an exception");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHost ConfigHost()
        {
            return new HostBuilder()
                .ConfigureServices(AppStartup.InitApplication)
                .ConfigureLogging(log =>
                {
                    log.ClearProviders();
                    log.AddConsole();
                })
                .UseSerilog()
                .Build();
        }
    }
}
