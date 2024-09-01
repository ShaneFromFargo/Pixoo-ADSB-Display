using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.ADSB.Models
{
    public class FlightRadarSearch
    {
        public List<Result> results { get; set; }
        public Stats stats { get; set; }

        public class Count
        {
            public int airport { get; set; }
            public int @operator { get; set; }
            public int live { get; set; }
            public int schedule { get; set; }
            public int aircraft { get; set; }
        }

        public class Detail
        {
            public string iata { get; set; }
            public string logo { get; set; }
            public double? lat { get; set; }
            public double? lon { get; set; }
            public string schd_from { get; set; }
            public string schd_to { get; set; }
            public string ac_type { get; set; }
            public string route { get; set; }
            public string reg { get; set; }
            public string callsign { get; set; }
            public string flight { get; set; }
            public string @operator { get; set; }
        }

        public class Result
        {
            public string id { get; set; }
            public string label { get; set; }
            public Detail detail { get; set; }
            public string type { get; set; }
            public string match { get; set; }
            public string name { get; set; }
        }


        public class Stats
        {
            public Total total { get; set; }
            public Count count { get; set; }
        }

        public class Total
        {
            public int all { get; set; }
            public int airport { get; set; }
            public int @operator { get; set; }
            public int live { get; set; }
            public int schedule { get; set; }
            public int aircraft { get; set; }
        }
    }
}
