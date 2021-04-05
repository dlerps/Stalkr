using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Stalkr.In;
using Stalkr.Out;

namespace Stalkr.Core
{
    public class StalkrService : IStalkrService
    {
        private readonly IChecksumStalkr _checksumService;

        private readonly IContentStalkr _contentService;

        private readonly IChecksumMemory _checksumMemory;

        private readonly ISpamr _spamr;

        private readonly ILogger<StalkrService> _logger;

        public StalkrService(
            IChecksumStalkr checksumService, 
            IContentStalkr contentService, 
            IChecksumMemory checksumMemory, 
            ISpamr spamr, 
            ILogger<StalkrService> logger)
        {
            _checksumService = checksumService;
            _contentService = contentService;
            _checksumMemory = checksumMemory;
            _spamr = spamr;
            _logger = logger;
        }

        public async Task GoStalking(CancellationToken cancellationToken = default)
        {
            var content = await _contentService.ReadContent();
            var checksum = _checksumService.GetSha256Digest(content);
            
            if (await _checksumMemory.ContainsChecksum(checksum))
            {
                _logger.LogInformation("{Checksum} did not change", checksum);
                await _spamr.NotifyChannels(false);
                
                return;
            }

            _logger.LogInformation("{Checksum} is new. Notifying registered channels", checksum);

            await _checksumMemory.Memorise(checksum);
            await _spamr.NotifyChannels(true);
        }
    }
}