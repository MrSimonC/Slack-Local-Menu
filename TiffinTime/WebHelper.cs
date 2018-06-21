using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TiffinTime
{
    class WebHelper
    {
        HtmlWeb web = new HtmlWeb();
        
        public string MenuGetOptionText(string url, string xpath)
        {
            var doc = web.Load(url);
            var htmlMenuOption = doc.DocumentNode.SelectSingleNode(xpath).InnerText;
            return HttpUtility.HtmlDecode(htmlMenuOption);
        }

        public string GetMenuImageUrl(string url, string xpath)
        {
            var doc = web.Load(url);
            return doc.DocumentNode.SelectSingleNode(xpath).Attributes["src"].Value;
        }
    }
}
