using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Stalkr.Core
{
    public class Runnr : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public Runnr(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                using var scope = _serviceProvider.CreateScope();
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Runnr>>();

                var interval = 1000;
                
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    var config = scope.ServiceProvider.GetRequiredService<StalkrConfiguration>();
                    var stalkrService = scope.ServiceProvider.GetRequiredService<IStalkrService>();

                    interval = config.Interval;

                    await stalkrService.GoStalking(cancellationToken);
                    await Task.Delay(interval, cancellationToken);

                    logger.LogInformation("Finished Stalking cycle. Repeating in {Interval}ms", interval);
                }
                catch (OperationCanceledException)
                {
                    logger.LogInformation("Stopping Stalkr upon request");
                    break;
                }
                catch (Exception e)
                {
                    logger.LogError(e, "Stalkr encountered an {ExceptionType} error", e.GetType().Name);
                    
                    // ReSharper disable once MethodSupportsCancellation
                    await Task.Delay(interval);
                }
            }
        }
    }
}