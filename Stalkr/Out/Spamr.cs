using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Stalkr.Out.Channels;

namespace Stalkr.Out
{
    public class Spamr : ISpamr
    {
        private readonly IEnumerable<ISpamChannel> _channels;

        public Spamr(IServiceProvider serviceProvider)
        {
            _channels = serviceProvider.GetServices<ISpamChannel>();
        }
        
        public Task NotifyChannels(bool changeHappened)
        {
            var notificationTasks = new List<Task>(_channels.Count());
            
            foreach (var spamChannel in _channels)
                notificationTasks.Add(spamChannel.Notify(changeHappened));

            return Task.WhenAll(notificationTasks);
        }
    }
}