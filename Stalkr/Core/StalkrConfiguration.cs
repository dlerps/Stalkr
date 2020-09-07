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

        public bool UseInnerHtml { get; set; }

        public static StalkrConfiguration FromConfiguration(IConfiguration configuration)
        {
            Boolean.TryParse(configuration["UseInnerHtml"], out var useInnerHtml);
            
            return new StalkrConfiguration()
            {
                XPathSelector = configuration["XPathSelector"],
                WebsiteAddress = configuration["WebSiteAddress"],
                Title = configuration["Title"] ?? configuration["WebSiteAddress"],
                Interval = ReadInterval(configuration),
                UseInnerHtml = useInnerHtml
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