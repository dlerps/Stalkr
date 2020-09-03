using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stalkr.Core;
using Stalkr.In;

namespace Stalkr
{
    // ReSharper disable once ClassNeverInstantiated.Global
    class Program
    {
        static async Task Main(string[] args)
        {
            var services = AppStartup.InitApplication();

            using var scope = services.CreateScope();
            var stalkr = scope.ServiceProvider.GetRequiredService<IStalkrService>();

            await stalkr.GoStalking();
        }
    }
}
