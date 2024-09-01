using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.ADSB.Models
{
    public class DisplayInformation
    {
        public string FlightNumber { get; set; }
        public int Altitude { get; set; }
        public double GroundSpeed { get; set; }
        public string Airline { get; set; }
        public string AirlineLogo { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public string FlightDescription { get; set; }
        public string AircraftType { get; set; }
    }
}
