using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Stalkr.Core
{
    public class Runnr
    {
        private readonly IServiceProvider _serviceProvider;

        public Runnr(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public async Task RunStalkr(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    
                    using var scope = _serviceProvider.CreateScope();

                    var config = scope.ServiceProvider.GetRequiredService<StalkrConfiguration>();
                    var stalkrService = scope.ServiceProvider.GetRequiredService<IStalkrService>();

                    await stalkrService.GoStalking(cancellationToken);
                    await Task.Delay(config.Interval, cancellationToken);
                }
            }
            catch (OperationCanceledException) { }
            
            Console.WriteLine("Shutting down now... good bye!");
        }
    }
}