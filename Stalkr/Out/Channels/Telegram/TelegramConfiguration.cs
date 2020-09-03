using Microsoft.Extensions.Configuration;

namespace Stalkr.Out.Channels.Telegram
{
    public class TelegramConfiguration : ChannelConfiguration
    {
        public string BotToken { get; }

        public string ChatId { get; set; }

        public string BaseAddress { get; set; }
        
        public TelegramConfiguration(IConfigurationSection configuration) 
            : base(configuration)
        {
            BotToken = configuration["BotToken"];
            ChatId = configuration["ChatId"];
            BaseAddress = configuration["BaseAddress"];
        }
    }
}