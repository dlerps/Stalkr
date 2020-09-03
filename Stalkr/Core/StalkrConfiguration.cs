using System;
using Microsoft.Extensions.Configuration;

namespace Stalkr.Core
{
    public class StalkrConfiguration
    {
        private const int DefaultInterval = 120000;
        
        public string Title { get; set; }
        
        public string WebsiteAddress { get; set; }
        
        public string XPathSelector { get; set; }
        
        public int Interval { get; set; }

        public static StalkrConfiguration FromConfiguration(IConfiguration configuration)
        {
            return new StalkrConfiguration()
            {
                XPathSelector = configuration["XPathSelector"],
                WebsiteAddress = configuration["WebSiteAddress"],
                Title = configuration["Title"] ?? configuration["WebSiteAddress"],
                Interval = ReadInterval(configuration)
            };
        }

        private static int ReadInterval(IConfiguration config)
        {
            return Int32.TryParse(config["Interval"], out var interval) && interval > 0
                ? interval
                : DefaultInterval;
        }
    }
}