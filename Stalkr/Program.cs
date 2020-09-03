using System;
using System.Threading;
using System.Threading.Tasks;
using Stalkr.Core;

namespace Stalkr
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class Program
    {
        private static CancellationToken _cancellationToken;
        
        static async Task Main(string[] args)
        {
            var services = AppStartup.InitApplication();

            var runnr = new Runnr(services);
            await runnr.RunStalkr(_cancellationToken);
        }

        private static void RegisterShutdownEvent()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = cancellationTokenSource.Token;

            AppDomain.CurrentDomain.ProcessExit += (_, __) =>
            {
                cancellationTokenSource.Cancel();
            };
        }
    }
}
