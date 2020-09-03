using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Stalkr.Core;
using Stalkr.In;

namespace Stalkr.Out.Channels
{
    public class ConsoleChannel : ISpamChannel
    {
        private readonly IChecksumMemory _checksumMemory;

        private readonly StalkrConfiguration _stalkrConfiguration;

        private readonly ChannelConfiguration _channelConfiguration;

        private Stopwatch _timeSinceNoChangeSpam;

        public ConsoleChannel(
            IChecksumMemory checksumMemory, 
            StalkrConfiguration stalkrConfiguration,
            IConfiguration configuration)
        {
            _stalkrConfiguration = stalkrConfiguration;
            _checksumMemory = checksumMemory;
            _channelConfiguration = new ChannelConfiguration(configuration.GetSection("Console"));
        }
        
        public Task Notify(bool changeHappened)
        {
            return changeHappened ? HandleChange() : HandleNoChange();
        }
        
        private Task HandleNoChange()
        {
            if (_timeSinceNoChangeSpam == null
                || _timeSinceNoChangeSpam.ElapsedMilliseconds >= _channelConfiguration.NoChangeNotificationInterval)
            {
                Console.WriteLine($"Nothing changed at {_stalkrConfiguration.Title}. Checksum is still: {_checksumMemory.LastChecksum}");
                _timeSinceNoChangeSpam = Stopwatch.StartNew();
            }
            
            return Task.CompletedTask;
        }
        
        private Task HandleChange()
        {
            Console.WriteLine($"Something changed!! at {_stalkrConfiguration.Title}. Checksum is now: {_checksumMemory.LastChecksum}");
            return Task.CompletedTask;
        }
    }
}