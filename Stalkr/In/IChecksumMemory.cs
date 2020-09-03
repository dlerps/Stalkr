using System.Threading.Tasks;

namespace Stalkr.In
{
    public interface IChecksumMemory
    {
        Task Memorise(string checksum);

        Task<bool> ContainsChecksum(string checksum);
    }
}