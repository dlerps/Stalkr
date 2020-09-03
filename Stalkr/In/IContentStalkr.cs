using System.Threading.Tasks;

namespace Stalkr.In
{
    public interface IContentStalkr
    {
        Task<string> ReadContent();
    }
}