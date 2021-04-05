using Serilog.Core;
using Serilog.Core.Enrichers;

namespace Stalkr.Core.Logging
{
    public static class LoggingEnrichers
    {
        public static ILogEventEnricher AppEnricher =>
            new PropertyEnricher("Application", $"Stalkr: {AppStartup.Configuration["Title"]}");
    }
}