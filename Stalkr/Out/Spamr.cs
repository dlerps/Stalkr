using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stalkr.Out.Channels;

namespace Stalkr.Out
{
    public class Spamr : ISpamr
    {
        private readonly IEnumerable<ISpamChannel> _channels;

        private readonly ILogger<Spamr> _logger;

        public Spamr(IEnumerable<ISpamChannel> channels, ILogger<Spamr> logger)
        {
            _channels = channels;
            _logger = logger;
        }
        
        public Task NotifyChannels(bool changeHappened)
        {
            var notificationTasks = new List<Task>(_channels.Count());
            
            foreach (var spamChannel in _channels)
                notificationTasks.Add(spamChannel.Notify(changeHappened));
            
            if (changeHappened)
                _logger.LogInformation("Notified {ChannelCount} channels about change", _channels.Count());
            else
                _logger.LogInformation("Notified {ChannelCount} channels with no change", _channels.Count());

            return Task.WhenAll(notificationTasks);
        }
    }
}