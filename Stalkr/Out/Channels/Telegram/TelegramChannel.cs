using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Refit;
using Stalkr.Core;
using Stalkr.In;
using Stalkr.Out.Channels.Telegram.Api;
using Stalkr.Out.Channels.Telegram.Api.Model;

namespace Stalkr.Out.Channels.Telegram
{
    public class TelegramChannel : ISpamChannel
    {
        private static Stopwatch _timeSinceNoChangeSpam;
        
        private readonly IChecksumMemory _checksumMemory;

        private readonly StalkrConfiguration _stalkrConfiguration;

        private readonly TelegramConfiguration _channelConfiguration;

        private readonly ILogger<TelegramChannel> _logger;

        private string LastChecksumShort => _checksumMemory.LastChecksum.Substring(0, 16);
        
        public TelegramChannel(
            IChecksumMemory checksumMemory, 
            StalkrConfiguration stalkrConfiguration,
            IConfiguration configuration,
            ILogger<TelegramChannel> logger)
        {
            _stalkrConfiguration = stalkrConfiguration;
            _checksumMemory = checksumMemory;
            _logger = logger;
            
            _channelConfiguration = new TelegramConfiguration(configuration.GetSection("Telegram"));
        }
        
        public Task Notify(bool changeHappened)
        {
            return changeHappened ? HandleChange() : HandleNoChange();
        }
        
        private async Task HandleNoChange()
        {
            if (_timeSinceNoChangeSpam == null
                || _timeSinceNoChangeSpam.ElapsedMilliseconds >= _channelConfiguration.NoChangeNotificationInterval)
            {
                var minutes = _channelConfiguration.NoChangeNotificationInterval / (1000 * 60);
                var message =
                    $"{minutes} minute Update on *{_stalkrConfiguration.Title}*: Still no change\\. " +
                    $"Checksum is _{LastChecksumShort}_";
                
                await SendMessage(message, false);
                
                _timeSinceNoChangeSpam = Stopwatch.StartNew();
            }
        }
        
        private async Task HandleChange()
        {
            var message =
                $"``` " +
                $"There is an update\\! " +
                $"``` " +
                $"*{_stalkrConfiguration.Title}* changed to checksum: _{LastChecksumShort}_ " +
                $"[open]({_stalkrConfiguration.WebsiteAddress})";
            
            await SendMessage(message, true);
            
            _timeSinceNoChangeSpam = Stopwatch.StartNew();
        }

        private async Task SendMessage(string messageText, bool notify)
        {
            var telegramApi = RestService.For<ITelegramApi>(_channelConfiguration.BaseAddress);
            var message = new TelegramMessage()
            {
                Text = messageText,
                ChatId = _channelConfiguration.ChatId,
                DisableNotification = !notify
            };

            await telegramApi.SendMessage(_channelConfiguration.BotToken, message);
            
            _logger.LogInformation("Sent message to chat {TelegramChatId}", _channelConfiguration.ChatId);
        }
    }
}