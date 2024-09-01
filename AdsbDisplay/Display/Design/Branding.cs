using AdsbDisplay.ADSB.Models;
using AdsbDisplay.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdsbDisplay.Display.Design
{
    public class Branding
    {
        public static AirlineBranding DeltaBranding()
        {
            AirlineBranding airlineBranding = new AirlineBranding()
            {
                FlightNumber = new FlightNumber()
                {
                    Font = 4,
                    Color = "#C01933",
                    OffsetX = 4,
                    OffsetY = 4
                },
                Arrival = new Arrival()
                {
                    Font = 4,
                    Color = "#FFFFFF",
                    OffsetX = 39,
                    OffsetY = 22
                },
                Departure = new Departure()
                {
                    Font = 4,
                    Color = "#FFFFFF",
                    OffsetX = 2,
                    OffsetY = 22
                },
                Altitude = new Altitude()
                {
                    Font = 4,
                    Color = "#C01933",
                    OffsetX = 32
                },
                BackgroundImage = "Delta.png",
                Name = "Delta"
            };

            return airlineBranding;
        }
        public static AirlineBranding SouthwestBranding()
        {
            AirlineBranding airlineBranding = new AirlineBranding()
            {
                FlightNumber = new FlightNumber()
                {
                    Font = 4,
                    Color = "#F9B612",
                    OffsetX = 4,
                    OffsetY = 4
                },
                Arrival = new Arrival()
                {
                    Font = 4,
                    Color = "#FF0000",
                    OffsetX = 38,
                    OffsetY = 27
                },
                Departure = new Departure()
                {
                    Font = 4,
                    Color = "#FF0000",
                    OffsetX = 2,
                    OffsetY = 27
                },
                Altitude = new Altitude()
                {
                    Font = 4,
                    Color = "#F9B612",
                },
                BackgroundImage = "SW.png",
                Name = "Southwest"
            };
            return airlineBranding;

        }

        public static AirlineBranding AmericanBranding()
        {
            AirlineBranding airlineBranding = new AirlineBranding()
            {
                FlightNumber = new FlightNumber()
                {
                    Font = 4,
                    Color = "#C30019",
                    OffsetX = 4,
                    OffsetY = 4
                },
                Arrival = new Arrival()
                {
                    Font = 4,
                    Color = "#36495A",
                    OffsetX = 38,
                    OffsetY = 27
                },
                Departure = new Departure()
                {
                    Font = 4,
                    Color = "#36495A",
                    OffsetX = 2,
                    OffsetY = 27
                },
                Altitude = new Altitude()
                {
                    Font = 4,
                    Color = "#C30019",
                },
                BackgroundImage = "AA.png",
                Name = "American"
            };
            return airlineBranding;

        }

        public static AirlineBranding JSXBranding()
        {
            AirlineBranding airlineBranding = new AirlineBranding()
            {
                FlightNumber = new FlightNumber()
                {
                    Font = 4,
                    Color = "#e51d23",
                    OffsetX = 4,
                    OffsetY = 4
                },
                Arrival = new Arrival()
                {
                    Font = 4,
                    Color = "#000000",
                    OffsetX = 38,
                    OffsetY = 27
                },
                Departure = new Departure()
                {
                    Font = 4,
                    Color = "#000000",
                    OffsetX = 2,
                    OffsetY = 27
                },
                Altitude = new Altitude()
                {
                    Font = 4,
                    Color = "#e51d23",
                },
                BackgroundImage = "JSX.png",
                Name = "JSX"
            };
            return airlineBranding;

        }

        public static AirlineBranding GenericBranding()
        {
            AirlineBranding airlineBranding = new AirlineBranding()
            {
                FlightNumber = new FlightNumber()
                {
                    Font = 4,
                    Color = "#000000",
                    OffsetX = 4,
                    OffsetY = 4
                },
                Arrival = new Arrival()
                {
                    Font = 4,
                    Color = "#FFFFFF",
                    OffsetX = 38,
                    OffsetY = 27
                },
                Departure = new Departure()
                {
                    Font = 4,
                    Color = "#FFFFFF",
                    OffsetX = 2,
                    OffsetY = 27
                },
                Altitude = new Altitude()
                {
                    Font = 4,
                    Color = "#000000",
                },
                BackgroundImage = "Generic.png",
                Name = "None"
            };
            return airlineBranding;
        }

        public static AirlineBranding GeneralAviation()
        {
            AirlineBranding airlineBranding = new AirlineBranding()
            {
                FlightNumber = new FlightNumber()
                {
                    Font = 4,
                    Color = "#000000",
                    OffsetX = 4,
                    OffsetY = 4
                },
                Arrival = new Arrival()
                {
                    Font = 4,
                    Color = "#FF0000",
                    OffsetX = 32,
                    OffsetY = 45
                },
                Departure = new Departure()
                {
                    Font = 4,
                    Color = "#FF0000",
                    OffsetX = 4,
                    OffsetY = 45
                },
                BackgroundImage = "Default.png",
                Name = "GA"
            };
            return airlineBranding;
        }
    }
}
