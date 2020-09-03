using System.Threading.Tasks;

namespace Stalkr.In
{
    public interface IChecksumMemory
    {
        string LastChecksum { get; }

        Task Memorise(string checksum);

        Task<bool> ContainsChecksum(string checksum);
    }
}