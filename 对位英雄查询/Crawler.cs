using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Windows.Forms;
using System.Drawing;

namespace 对位英雄查询
{
    enum Position
    {
        Top,
        Jungle,
        Mid,
        Bottom,
        Support
    }

    class Crawler
    {
        public static Dictionary<Position, List<string>> PositionChampions = new Dictionary<Position, List<string>>();
        public static WebHeaderCollection headers = new WebHeaderCollection()
        {
            { "Accept-Language","zh-CN,zh;q=0.9"}
        };

        public static void GetPostionChampions()
        {
            string url = "http://www.op.gg/champion/statistics/";
            HttpWebRequest request = WebRequest.CreateHttp(url);
            request.Headers = headers;
            var stream = request.GetResponse().GetResponseStream();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(stream);
            var nodes=doc.DocumentNode.SelectNodes("//div[@class=\"champion - index - table__name\"]");
            foreach (var node in nodes)
            {
                Console.WriteLine(node.InnerText);
            }
     
        }
    }
}
