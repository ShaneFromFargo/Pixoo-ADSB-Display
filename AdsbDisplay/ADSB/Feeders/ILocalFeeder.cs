using AdsbDisplay.ADSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.ADSB.Feeders
{
    public interface ILocalFeeder
    {
        Task<SkyResponse> GetSky();
    }
}
