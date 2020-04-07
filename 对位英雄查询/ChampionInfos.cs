using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 对位英雄查询
{
    class Champion
    {
        public int Index { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

    }

    class ChampionInfos
    {
        public List<Champion> Top { get; set; }
        public List<Champion> Jungle { get; set; }
        public List<Champion> Mid { get; set; }
        public List<Champion> Adc { get; set; }
        public List<Champion> Support { get; set; }
    }
}
