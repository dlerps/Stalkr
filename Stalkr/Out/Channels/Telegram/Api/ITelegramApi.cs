using System.Threading.Tasks;
using Refit;
using Stalkr.Out.Channels.Telegram.Api.Model;

namespace Stalkr.Out.Channels.Telegram.Api
{
    public interface ITelegramApi
    {
        [Post("/bot{botToken}/sendMessage")]
        Task<TelegramMessage> SendMessage([AliasAs("botToken")] string token, [Body] TelegramMessage message);
        
        [Get("/bot{botToken}/getChat")]
        Task<TelegramChat> GetChat([AliasAs("botToken")] string token, [AliasAs("chat_id")] string chatId);
    }
}