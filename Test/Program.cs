using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;
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
        static void Main()
        {
            GetPostionChampions();
        }

        public static Dictionary<Position, List<string>> PositionChampions = new Dictionary<Position, List<string>>();
        public static WebHeaderCollection headers = new WebHeaderCollection()
        {
            { "Accept-Language","zh-CN,zh;q=0.9"}
        };

        public static void GetPostionChampions()
        {
            string home = "http://www.op.gg/champion/statistics/";
            HttpWebRequest request = WebRequest.CreateHttp(home);
            request.Headers = headers;
            var stream = request.GetResponse().GetResponseStream();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(stream, Encoding.UTF8);
            ChampionInfos championInfos = new ChampionInfos();
            var properties = championInfos.GetType().GetProperties();
            foreach (var property in properties)
            {
                string position = property.Name;
                var nodes = doc.DocumentNode.SelectNodes($"//tbody[@class=\"tabItem champion-trend-tier-{position.ToUpper()}\"]/tr");


                List<Champion> champions = new List<Champion>();
                foreach (var node in nodes)
                {
                    var index = node.SelectSingleNode(".//i").GetAttributeValue("class", string.Empty).Split('-').Last();
                    var name = node.SelectSingleNode(".//div[@class=\"champion-index-table__name\"]").InnerText;
                    var url = node.SelectSingleNode(".//a").GetAttributeValue("href", string.Empty);
                    Champion champion = new Champion()
                    {
                        Index = Convert.ToInt32(index),
                        Name = name,
                        Url = url
                    };
                    champions.Add(champion);
                }
                property.SetValue(championInfos, champions);
            }


            //Console.WriteLine(nodes.Count);

        }
    }
}
