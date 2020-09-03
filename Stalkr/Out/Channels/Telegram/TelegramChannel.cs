using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Refit;
using Stalkr.Core;
using Stalkr.In;
using Stalkr.Out.Channels.Telegram.Api;
using Stalkr.Out.Channels.Telegram.Api.Model;

namespace Stalkr.Out.Channels.Telegram
{
    public class TelegramChannel : ISpamChannel
    {
        private readonly IChecksumMemory _checksumMemory;

        private readonly StalkrConfiguration _stalkrConfiguration;

        private readonly TelegramConfiguration _channelConfiguration;

        private static Stopwatch _timeSinceNoChangeSpam;

        public TelegramChannel(
            IChecksumMemory checksumMemory, 
            StalkrConfiguration stalkrConfiguration,
            IConfiguration configuration)
        {
            _stalkrConfiguration = stalkrConfiguration;
            _checksumMemory = checksumMemory;
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
                await SendMessage(
                    $"Nothing changed at {_stalkrConfiguration.Title}. Checksum is still: {_checksumMemory.LastChecksum}",
                    false
                );
                
                _timeSinceNoChangeSpam = Stopwatch.StartNew();
            }
        }
        
        private async Task HandleChange()
        {
            await SendMessage(
                $"Something changed!! at {_stalkrConfiguration.Title}. Checksum is now: {_checksumMemory.LastChecksum}",
                true
            );
            
            _timeSinceNoChangeSpam = Stopwatch.StartNew();
        }

        private Task SendMessage(string messageText, bool notify)
        {
            var telegramApi = RestService.For<ITelegramApi>(_channelConfiguration.BaseAddress);
            var message = new TelegramMessage()
            {
                Text = messageText,
                ChatId = _channelConfiguration.ChatId,
                DisableNotification = !notify
            };

            return telegramApi.SendMessage(_channelConfiguration.BotToken, message);
        }
    }
}