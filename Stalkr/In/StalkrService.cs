using System;
using System.Threading.Tasks;

namespace Stalkr.In
{
    public class StalkrService : IStalkrService
    {
        private readonly IChecksumStalkr _checksumService;

        private readonly IContentStalkr _contentService;

        private readonly IChecksumMemory _checksumMemory;

        public StalkrService(
            IChecksumStalkr checksumService, 
            IContentStalkr contentService,
            IChecksumMemory checksumMemory)
        {
            _contentService = contentService;
            _checksumService = checksumService;
            _checksumMemory = checksumMemory;
        }

        public async Task GoStalking()
        {
            var content = await _contentService.ReadContent();
            var checksum = _checksumService.GetSha256Digest(content);

            Console.WriteLine($"Current checksum: {checksum}");
            
            if (await _checksumMemory.ContainsChecksum(checksum))
            {
                Console.WriteLine($"Nothing new...");
                return;
            }

            await _checksumMemory.Memorise(checksum);
            Console.WriteLine("There has been an update!!!");
        }
    }
}