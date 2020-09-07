using System;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Stalkr.Core;

namespace Stalkr.In
{
    public class ContentStalkr : IContentStalkr
    {
        private readonly StalkrConfiguration _config;

        public ContentStalkr(StalkrConfiguration config)
        {
            _config = config;
        }

        public async Task<string> ReadContent()
        {
            var webReader = new HtmlWeb
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:80.0) Gecko/20100101 Firefox/80.0"
            };

            var dom = await webReader.LoadFromWebAsync(_config.WebsiteAddress, Encoding.UTF8);
            
            var contentNode = GetContentNode(dom);

            return _config.UseInnerHtml ? contentNode.InnerHtml : contentNode.WriteTo();
        }

        private HtmlNode GetContentNode(HtmlDocument dom)
        {
            return String.IsNullOrEmpty(_config.XPathSelector)
                ? dom.DocumentNode 
                : dom.DocumentNode.SelectSingleNode(_config.XPathSelector);
        }
    }
}