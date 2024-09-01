using AdsbDisplay.ADSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdsbDisplay.ADSB.Models.SkyResponse;

namespace AdsbDisplay.ADSB.Feeders
{
    public interface IAPIFeeder
    {
        Task<SkyResponse> GetSky();
        Task<Ac> GetPlane(string callSign);
    }
}
