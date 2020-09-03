using System.Threading.Tasks;

namespace Stalkr.Out.Channels
{
    public interface ISpamChannel
    {
        Task Notify(bool changeHappened);
    }
}