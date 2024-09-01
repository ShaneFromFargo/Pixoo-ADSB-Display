using AdsbDisplay.ADSB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.ADSB
{
    public interface IADSB
    {
        Task<DisplayInformation> GetPlane();
        Task<DisplayInformation> Test(string flightNumber);
        Task<DisplayInformation> GetRandomPlane();
        bool WithinRadius(double latitude, double longitude, int radius);
    }
}
