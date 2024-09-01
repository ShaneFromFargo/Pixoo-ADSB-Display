using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.Models
{
    public class AirlineBranding
    {
        public FlightNumber FlightNumber { get; set; }
        public Departure Departure { get; set; }
        public Arrival Arrival { get; set; }
        public string BackgroundImage { get; set; }
        public string Name { get; set; }
        public Altitude Altitude { get; set; }
    }
    public class FlightNumber
    {
        public int Font { get; set; }
        public string Color { get; set; }
        public int OffsetX { get; set; } = 4;
        public int OffsetY { get; set; } = 4;
    }

    public class Departure
    {
        public int Font { get; set; }
        public string Color { get; set; }
        public int OffsetX { get; set; } = 10;
        public int OffsetY { get; set; } = 45;
    }

    public class Arrival
    {
        public int Font { get; set; }
        public string Color { get; set; }
        public int OffsetX { get; set; } = 10;
        public int OffsetY { get; set; } = 45;
    }

    public class Altitude
    {
        public int Font { get; set; }
        public string Color { get; set; }
        public int OffsetX { get; set; } = 19;
        public int OffsetY { get; set; } = 46;
    }
}
