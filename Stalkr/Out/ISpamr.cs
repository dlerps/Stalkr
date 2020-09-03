using System.Threading.Tasks;

namespace Stalkr.Out
{
    public interface ISpamr
    {
        Task NotifyChannels(bool changeHappened);
    }
}