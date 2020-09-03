using Newtonsoft.Json;

namespace Stalkr.Out.Channels.Telegram.Api.Model
{
    public class TelegramMessage
    {
        [JsonProperty("chat_id")]
        public string ChatId { get; set; }
        
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("disable_notification")]
        public bool DisableNotification { get; set; } = false;
    }
}