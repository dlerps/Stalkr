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
            var webReader = new HtmlWeb();
            var dom = await webReader.LoadFromWebAsync(_config.WebsiteAddress, Encoding.UTF8);
            
            var contentNode = GetContentNode(dom);

            return contentNode.WriteTo();
        }

        private HtmlNode GetContentNode(HtmlDocument dom)
        {
            return String.IsNullOrEmpty(_config.XPathSelector)
                ? dom.DocumentNode 
                : dom.DocumentNode.SelectSingleNode(_config.XPathSelector);
        }
    }
}