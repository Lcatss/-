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
using System.Data;

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
        private static Uri home = new Uri("http://www.op.gg/");
        private static WebHeaderCollection headers = new WebHeaderCollection()
        {
            { "Accept-Language","zh-CN,zh;q=0.9"}
        };

        public static ChampionInfos GetPostionChampions()
        {
            var page = new Uri(home, "/champion/statistics/");
            HttpWebRequest request = WebRequest.CreateHttp(page);
            request.Headers = headers;
            var stream = request.GetResponse().GetResponseStream();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(stream, Encoding.UTF8);

            ChampionInfos championInfos = new ChampionInfos();
            var properties = championInfos.GetType().GetProperties();
            foreach (var property in properties)
            {
                string position = property.Name;
                var championNodes = doc.DocumentNode.SelectNodes($"//tbody[@class=\"tabItem champion-trend-tier-{position.ToUpper()}\"]/tr");

                List<Champion> champions = new List<Champion>();
                foreach (var node in championNodes)
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
            return championInfos;
        }

        public static DataTable GetCounterChampions(string url, ImageList.ImageCollection collection)
        {
            var position = url.Split('/').Last();

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("icon", typeof(Image));
            dataTable.Columns.Add("英雄", typeof(string));
            dataTable.Columns.Add("胜率", typeof(double));
            dataTable.Columns.Add("线杀率", typeof(double));

            var page = new Uri(home, url + "/matchup?");
            var request = WebRequest.CreateHttp(page);
            request.Headers = headers;
            var stream = request.GetResponse().GetResponseStream();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(stream, Encoding.UTF8);

            var championNodes = doc.DocumentNode.SelectNodes("//div[@class=\"champion-matchup-list__champion\"]");
            int id = Convert.ToInt32(doc.DocumentNode.SelectSingleNode("//div[@class=\"champion-matchup-champion\"]/a").GetAttributeValue("href", string.Empty).Split('=').Last());
            List<Task> tasks = new List<Task>();
            foreach (var node in championNodes)
            {
                Task t = PaserNodeAsync(position, dataTable, id, node, collection);
                tasks.Add(t);
            }
            Task.WaitAll(tasks.ToArray());
            return dataTable;

        }

        private async static Task PaserNodeAsync(string position, DataTable dataTable, int id, HtmlNode node, ImageList.ImageCollection collection)
        {
            int index = Convert.ToInt32(node.SelectSingleNode(".//i").GetAttributeValue("class", string.Empty).Split('-').Last());
            int targetId = Convert.ToInt32(node.SelectSingleNode("..").GetAttributeValue("data-champion-id", string.Empty));
            double winRate = 1 - Convert.ToDouble(node.SelectSingleNode("..").GetAttributeValue("data-value-winrate", string.Empty));
            string Name = node.SelectSingleNode(".//span").InnerText;
            Uri counterPage = new Uri($"http://www.op.gg/champion/ajax/statistics/counterChampion/championId={id}&targetChampionId={targetId}&position={position}");
            var laneKillRate = await GetLaneKillRateAsync(counterPage);
            dataTable.Rows.Add(collection[index], Name, winRate, laneKillRate);
        }

        //获取两个英雄之间的对线数据
        private static async Task<double> GetLaneKillRateAsync(Uri uri)
        {
            var request = WebRequest.CreateHttp(uri);
            var response = await request.GetResponseAsync();
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.Load(response.GetResponseStream(), Encoding.UTF8);
            var laneKillRate = doc.DocumentNode.SelectSingleNode("//table[@class=\"champion-matchup-table\"]//td").InnerText.Trim().Trim('%');
            return 1 - Convert.ToDouble(laneKillRate) / 100;
        }
    }
}

