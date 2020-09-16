using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stalkr.In
{
    public class ChecksumMemory : IChecksumMemory
    {
        private const string EmptyChecksum = "123empty00000000001337";
        
        private readonly ISet<string> _memory;
        
        public string LastChecksum { get; private set; }

        public ChecksumMemory()
        {
            _memory = new HashSet<string>();
            LastChecksum = String.Empty;
        }

        public Task Memorise(string checksum)
        {
            checksum = PrepareChecksum(checksum);
            
            LastChecksum = checksum;
            _memory.Add(checksum);

            return Task.CompletedTask;
        }

        public Task<bool> ContainsChecksum(string checksum)
        {
            checksum = PrepareChecksum(checksum);
            
            var checksumExists = _memory.Contains(checksum);
            return Task.FromResult(checksumExists);
        }

        private static string PrepareChecksum(string checksum) 
            => String.IsNullOrEmpty(checksum) ? EmptyChecksum : checksum;
    }
}