using System;
using System.Threading.Tasks;
using Stalkr.Core;
using Stalkr.In;

namespace Stalkr.Out.Channels
{
    public class ConsoleChannel : ISpamChannel
    {
        private readonly IChecksumMemory _checksumMemory;

        private readonly StalkrConfiguration _configuration;

        public ConsoleChannel(IChecksumMemory checksumMemory, StalkrConfiguration configuration)
        {
            _configuration = configuration;
            _checksumMemory = checksumMemory;
        }
        
        public Task Notify(bool changeHappened)
        {
            return changeHappened ? HandleChange() : HandleNoChange();
        }
        
        private Task HandleNoChange()
        {
            Console.WriteLine($"Nothing changed at {_configuration.Title}. Checksum is still: {_checksumMemory.LastChecksum}");
            return Task.CompletedTask;
        }
        
        private Task HandleChange()
        {
            Console.WriteLine($"Something changed!! at {_configuration.Title}. Checksum is now: {_checksumMemory.LastChecksum}");
            return Task.CompletedTask;
        }
    }
}