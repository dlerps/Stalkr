using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stalkr.In;

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
            services.AddSingleton(stalkrConfiguration);
            services.AddSingleton(config);

            services.AddScoped<IChecksumStalkr, ChecksumStalkr>();
            services.AddScoped<IContentStalkr, ContentStalkr>();
            services.AddScoped<IStalkrService, StalkrService>();

            return services.BuildServiceProvider();
        }
    }
}