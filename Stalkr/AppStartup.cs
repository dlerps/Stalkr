using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stalkr.Core;
using Stalkr.In;
using Stalkr.Out;
using Stalkr.Out.Channels;

namespace Stalkr
{
    public static class AppStartup
    {
        public static IServiceProvider InitApplication()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", false)
                .AddEnvironmentVariables()
                .Build();

            var stalkrConfiguration = StalkrConfiguration.FromConfiguration(config);
            
            var services = new ServiceCollection();

            services.AddSingleton<IChecksumMemory, ChecksumMemory>();
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton(stalkrConfiguration);

            services.AddScoped<IChecksumStalkr, ChecksumStalkr>();
            services.AddScoped<IContentStalkr, ContentStalkr>();
            services.AddScoped<IStalkrService, StalkrService>();
            services.AddScoped<ISpamr, Spamr>();
            services.AddScoped<ISpamChannel, ConsoleChannel>();

            return services.BuildServiceProvider();
        }
    }
}