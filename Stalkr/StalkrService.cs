using System;
using System.Threading.Tasks;
using Stalkr.In;
using Stalkr.Out;

namespace Stalkr
{
    public class StalkrService : IStalkrService
    {
        private readonly IChecksumStalkr _checksumService;

        private readonly IContentStalkr _contentService;

        private readonly IChecksumMemory _checksumMemory;

        private readonly ISpamr _spamr;

        public StalkrService(
            IChecksumStalkr checksumService, 
            IContentStalkr contentService,
            IChecksumMemory checksumMemory,
            ISpamr spamr)
        {
            _contentService = contentService;
            _spamr = spamr;
            _checksumService = checksumService;
            _checksumMemory = checksumMemory;
        }

        public async Task GoStalking()
        {
            var content = await _contentService.ReadContent();
            var checksum = _checksumService.GetSha256Digest(content);
            
            if (await _checksumMemory.ContainsChecksum(checksum))
            {
                await _spamr.NotifyChannels(false);
                return;
            }

            await _checksumMemory.Memorise(checksum);
            await _spamr.NotifyChannels(true);
        }
    }
}