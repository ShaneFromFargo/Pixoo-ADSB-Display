using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdsbDisplay.ADSB.Models
{
    public class SkyResponse
    {
        private List<Ac> _ac;
        private List<Ac> _aircraft;

        public int total { get; set; }
        public long ctime { get; set; }
        public double now { get; set; }
        public int ptime { get; set; }
        //depending on the api its either ac or aircrafts. Custom converter populates both props
        public List<Ac> ac
        {
            get
            {
                if (_ac == null && _aircraft != null)
                {
                    _ac = new List<Ac>(_aircraft);
                }
                return _ac;
            }
            set
            {
                _ac = value;
            }
        }
        public List<Ac> aircraft
        {
            get
            {
                if (_aircraft == null && _ac != null)
                {
                    _aircraft = new List<Ac>(_ac);
                }
                return _aircraft;
            }
            set
            {
                _aircraft = value;
            }
        }

        public void EnsureAcAircraft()
        {
            if (ac == null && aircraft != null)
            {
                ac = new List<Ac>(aircraft);
            }
            else if (aircraft == null && ac != null)
            {
                aircraft = new List<Ac>(ac);
            }
        }



        public class Ac
        {
            public string hex { get; set; }
            public string type { get; set; }
            public string flight { get; set; }
            public string r { get; set; }
            public string t { get; set; }
            [JsonConverter(typeof(AltBaroConverter))]
            public string alt_baro { get; set; }
            public int alt_geom { get; set; }
            public double gs { get; set; }
            public double track { get; set; }
            public int baro_rate { get; set; }
            public string squawk { get; set; }
            public string emergency { get; set; }
            public string category { get; set; }
            public double nav_qnh { get; set; }
            public int nav_altitude_mcp { get; set; }
            public double nav_heading { get; set; }
            public double lat { get; set; }
            public double lon { get; set; }
            public int nic { get; set; }
            public int rc { get; set; }
            public double seen_pos { get; set; }
            public int version { get; set; }
            public int nic_baro { get; set; }
            public int nac_p { get; set; }
            public int nac_v { get; set; }
            public int sil { get; set; }
            public string sil_type { get; set; }
            public int gva { get; set; }
            public int sda { get; set; }
            public int alert { get; set; }
            public int spi { get; set; }
            public List<object> mlat { get; set; }
            public List<object> tisb { get; set; }
            public int messages { get; set; }
            public double seen { get; set; }
            public double rssi { get; set; }
        }



    }
}
