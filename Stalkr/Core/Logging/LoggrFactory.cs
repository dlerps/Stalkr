using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace Stalkr.Core.Logging
{
    public static class LoggrFactory
    {
        public static Logger CreateGlobalLogger()
        {
            AppStartup.LoadConfiguration();
            
            return new LoggerConfiguration()
                .WriteTo.Console()
                .WriteTo.Seq(
                    AppStartup.Configuration["Seq:IngestionEndpoint"], 
                    apiKey: AppStartup.Configuration["Seq:ApiKey"])
                .Enrich.WithMachineName()
                .Enrich.FromLogContext()
                .Enrich.WithDemystifiedStackTraces()
                .Enrich.With(LoggingEnrichers.AppEnricher)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .CreateLogger();
            ;
        }
    }
}