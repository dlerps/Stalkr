using System.Threading;
using System.Threading.Tasks;

namespace Stalkr.Core
{
    public interface IStalkrService
    {
        Task GoStalking(CancellationToken cancellationToken = default);
    }
}