using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stalkr.In
{
    public class ChecksumMemory : IChecksumMemory
    {
        private const string _emptyChecksum = "123empty";
        
        private readonly ISet<string> _memory;
        
        public string LastChecksum { get; private set; }

        public ChecksumMemory()
        {
            _memory = new HashSet<string>();
            LastChecksum = String.Empty;
        }

        public Task Memorise(string checksum)
        {
            if (String.IsNullOrEmpty(checksum))
                checksum = _emptyChecksum;

            LastChecksum = checksum;
            _memory.Add(checksum);

            return Task.CompletedTask;
        }

        public Task<bool> ContainsChecksum(string checksum)
        {
            var checksumExists = _memory.Contains(checksum);
            return Task.FromResult(checksumExists);
        }
    }
}