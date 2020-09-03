using System;
using Microsoft.Extensions.Configuration;

namespace Stalkr.Out.Channels
{
    public class ChannelConfiguration
    {
        private const int DefaultNoChangeNotificationInterval = 120000;
        
        public int NoChangeNotificationInterval { get; }
        
        public ChannelConfiguration(IConfigurationSection configuration)
        {
            NoChangeNotificationInterval = ReadNoChangeNotificationInterval(configuration);
        }
        
        private int ReadNoChangeNotificationInterval(IConfigurationSection config)
        {
            return config != null && Int32.TryParse(config["NoChangeNotificationInterval"], out var interval) && interval > 0
                ? interval
                : DefaultNoChangeNotificationInterval;
        }
    }
}