using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stalkr.Core;
using Stalkr.In;
using Stalkr.Out;
using Stalkr.Out.Channels;
using Stalkr.Out.Channels.Telegram;

namespace Stalkr
{
    public static class AppStartup
    {
        public static IConfiguration Configuration { get; private set; }
        
        public static void InitApplication(IServiceCollection services)
        {
            LoadConfiguration();
            
            var stalkrConfiguration = StalkrConfiguration.FromConfiguration(Configuration);

            services.AddHostedService<Runnr>();
            
            services.AddSingleton<IChecksumMemory, ChecksumMemory>();
            services.AddSingleton(Configuration);
            services.AddSingleton(stalkrConfiguration);

            services.AddScoped<IChecksumStalkr, ChecksumStalkr>();
            services.AddScoped<IContentStalkr, ContentStalkr>();
            services.AddScoped<IStalkrService, StalkrService>();
            services.AddScoped<ISpamr, Spamr>();
            services.AddScoped<ISpamChannel, ConsoleChannel>();
            services.AddScoped<ISpamChannel, TelegramChannel>();
        }

        public static void LoadConfiguration()
        {
            if (Configuration != null)
                return;
            
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile("appsettings.private.json", true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}