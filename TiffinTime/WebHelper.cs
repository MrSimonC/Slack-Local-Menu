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

        public string MenuGetOptionText(string url)
        {
            var doc = web.Load(url);
            var htmlMenuOption = doc.DocumentNode.SelectSingleNode("//*[@id=\"product\"]/div[1]/div[3]/div[1]/p[1]").InnerText;
            return HttpUtility.HtmlDecode(htmlMenuOption);
        }

        public string GetMenuImageUrl(string url)
        {
            var doc = web.Load(url);
            return doc.DocumentNode.SelectSingleNode("//*[@id=\"content\"]/div[1]/div/div/ul/li/a/img").Attributes["src"].Value;
        }
    }
}
